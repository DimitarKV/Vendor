#include <WiFiClientSecure.h>
#include <ArduinoJson.h>
#include <ezButton.h>

#include <string>
#include <map>
#include <vector>
#include <sstream>
#include <queue>

const char *SSID = "SSID";
const char *SSID_PASSWORD = "SSID_PASSWORD";

const char *VENDOR_USERS_SERVER_HOSTNAME = "192.168.1.104:7075";
const char *VENDOR_USERS_SERVER_ADDRESS = "192.168.1.104";
const int VENDOR_USERS_SERVER_PORT = 7075;

const char *VENDOR_MACHINES_SERVER_HOSTNAME = "192.168.1.104:7195";
const char *VENDOR_MACHINES_SERVER_ADDRESS = "192.168.1.104";
const int VENDOR_MACHINES_SERVER_PORT = 7195;

const std::string WHITESPACE = " \n\r\t\f\v";
#define DEBOUNCE_TIME 50

ezButton button1(5);
ezButton button2(18);
ezButton button3(19);
ezButton button4(21);
int led = 32;

std::string apiToken;

WiFiClientSecure client;

std::string ltrim(const std::string &s)
{
  size_t start = s.find_first_not_of(WHITESPACE);
  return (start == std::string::npos) ? "" : s.substr(start);
}

std::string rtrim(const std::string &s)
{
  size_t end = s.find_last_not_of(WHITESPACE);
  return (end == std::string::npos) ? "" : s.substr(0, end + 1);
}

std::string trim(const std::string &s)
{
  return rtrim(ltrim(s));
}

std::vector<std::string> splitAndTrim(std::string str, std::string delimiter)
{
  std::vector<std::string> strings;
  int index = str.find(delimiter);
  while (index != std::string::npos)
  {
    std::string newStr = str.substr(0, index);
    str.erase(0, str.find(delimiter) + delimiter.length());
    strings.push_back(newStr);

    index = str.find(delimiter);
  }
  if (str.length() > 0)
    strings.push_back(str);
  return strings;
}

class HttpResponse
{
public:
  HttpResponse(int responseCode, std::map<std::string, std::string> headers, std::string body)
  {
    _responseCode = responseCode;
    _headers = headers;
    _body = body;
  }

  bool isResponseCodeSuccessful()
  {
    return _responseCode >= 200 && _responseCode <= 299;
  }

  std::string getBody()
  {
    return _body;
  }

  std::map<std::string, std::string> getHeaders()
  {
    return _headers;
  }

  int getResponseCode()
  {
    return _responseCode;
  }

private:
  int _responseCode;
  std::map<std::string, std::string> _headers;
  std::string _body;
};

bool connectToServer(std::string serverAddress, int serverPort)
{
  if (!client.connect(serverAddress.c_str(), serverPort))
    return false;
  return true;
}

void closeConnection()
{
  client.stop();
}

HttpResponse readResponse()
{
  std::map<std::string, std::string> headers;
  std::string body;
  int responseCode = -1;
  bool head = false;

  while (client.connected())
  {
    std::string line = (client.readStringUntil('\n') + "\n").c_str();
    if (line == "\r\n")
      break;
    line = trim(line);
    if (!head)
    {
      responseCode = std::stoi(splitAndTrim(line, " ")[1]);
      head = true;
    }
    else
    {
      std::vector<std::string> parts = splitAndTrim(line, ": ");
      if (parts.size() >= 2)
        headers[parts[0]] = parts[1];
    }
  }

  int bodySize = 0;
  if (client.connected() && client.available())
  {
    std::string line = trim(std::string((client.readStringUntil('\n') + "\n").c_str()));
    std::stringstream ss;
    ss << std::hex << line;
    ss >> bodySize;
    char buff[bodySize];
    client.read((uint8_t *)&buff, bodySize);
    body = std::string(buff);
  }
  return HttpResponse(responseCode, headers, body);
}

DynamicJsonDocument deserializeJsonToDoc(std::string json)
{
  DynamicJsonDocument doc(16864);
  deserializeJson(doc, json);
  return doc;
}

std::string serializeDocToJson(DynamicJsonDocument doc)
{
  std::string json;
  serializeJson(doc, json);
  return json;
}

std::string login(char *username, char *password)
{
  if (!connectToServer(VENDOR_USERS_SERVER_ADDRESS, VENDOR_USERS_SERVER_PORT))
  {
    std::string empty = "";
    return empty;
  }
  DynamicJsonDocument doc(128);

  doc["username"] = username;
  doc["password"] = password;
  std::string body = serializeDocToJson(doc);

  client.println("POST /user/login HTTP/1.1");

  client.print("Host: ");
  client.println(VENDOR_USERS_SERVER_HOSTNAME);

  client.println("Content-Type: application/json");

  client.print("Content-Length: ");
  client.println(body.length());

  client.println();
  client.println(body.c_str());
  client.println();

  HttpResponse response = readResponse();
  DynamicJsonDocument responseDoc = deserializeJsonToDoc(response.getBody());
  std::string token = responseDoc["result"].as<std::string>();

  closeConnection();
  return token;
}

bool dropSpiral(long long spiralId)
{
  if (!connectToServer(VENDOR_MACHINES_SERVER_ADDRESS, VENDOR_MACHINES_SERVER_PORT))
    return false;

  client.print("GET /vending/drop/");
  client.print(spiralId);
  client.println(" HTTP/1.1");

  client.print("Host: ");
  client.println(VENDOR_MACHINES_SERVER_HOSTNAME);

  client.println("Content-Type: application/json");

  client.print("Authorization: Bearer ");
  client.println(apiToken.c_str());
  client.println();

  HttpResponse response = readResponse();
  DynamicJsonDocument responseDoc = deserializeJsonToDoc(response.getBody());
  bool isValid = responseDoc["isValid"];
  if (isValid)
  {
    int remainingQuantity = responseDoc["result"]["spirals"][0]["loads"];
    Serial.println(remainingQuantity);
  }

  closeConnection();
  return isValid;
}

void setup()
{
  Serial.begin(115200);

  button1.setDebounceTime(DEBOUNCE_TIME);
  button2.setDebounceTime(DEBOUNCE_TIME);
  button3.setDebounceTime(DEBOUNCE_TIME);
  button4.setDebounceTime(DEBOUNCE_TIME);
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);

  Serial.print("Attempting to connect to SSID: ");
  Serial.println(SSID);
  WiFi.begin(SSID, SSID_PASSWORD);

  // attempt to connect to Wifi network:
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(100);
  }

  Serial.print("Connected to ");
  Serial.println(SSID);

  // client.setCACert(ca);
  client.setInsecure();

  apiToken = login("admin", "admin");
  Serial.println(apiToken.c_str());
}

std::string commandBuffer;
std::queue<std::string> commands;
void handleSerial()
{
  while (Serial.available())
  {
    char c = Serial.read();
    Serial.print(c);
    commandBuffer += c;
    if (c == '\n')
    {
      commandBuffer = commandBuffer.substr(0, commandBuffer.find('\r'));
      commands.push(commandBuffer);
      commandBuffer = "";
    }
  }
}

void blink(int n)
{
  for (int i = 0; i < n; i++)
  {
    digitalWrite(led, HIGH);
    delay(70);
    digitalWrite(led, LOW);
    delay(70);
  }
}

void handleCommands()
{
  // handleSerial();
  // while (!commands.empty())
  // {
  //   std::string currentCommand = commands.front();
  //   char commandName = currentCommand[0];
  //   switch (commandName)
  //   {
  //   case 'D':
  //     Serial.println(dropSpiral(std::stoll(currentCommand.substr(1))) ? "Success!" : "Failure!");
  //     break;

  //   default:
  //     break;
  //   }
  //   commands.pop();
  // }

  if (button1.isPressed() || button2.isPressed() || button3.isPressed() || button4.isPressed())
  {
    digitalWrite(led, HIGH);
  }

  if (button1.isReleased())
  {
    digitalWrite(led, LOW);
    bool status = dropSpiral(1);
    Serial.println(status ? "Success!" : "Failure!");
    if (status)
      blink(2);
    else
      blink(3);
  }
  if (button2.isReleased())
  {
    digitalWrite(led, LOW);
    bool status = dropSpiral(2);
    Serial.println(status ? "Success!" : "Failure!");
    if (status)
      blink(2);
    else
      blink(3);
  }
  if (button3.isReleased())
  {
    digitalWrite(led, LOW);
    bool status = dropSpiral(3);

    Serial.println(status ? "Success!" : "Failure!");
    if (status)
      blink(2);
    else
      blink(3);
  }
  if (button4.isReleased())
  {
    digitalWrite(led, LOW);
    bool status = dropSpiral(4);

    Serial.println(status ? "Success!" : "Failure!");
    if (status)
      blink(2);
    else
      blink(3);
  }
}

void loop()
{
  button1.loop();
  button2.loop();
  button3.loop();
  button4.loop();

  handleCommands();
}
