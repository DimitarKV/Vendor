#include <WiFiClientSecure.h>
#include <ArduinoJson.h>
#include <string>
#include <map>
#include <vector>
#include <sstream>

const char *SSID = "SSID";
const char *SSID_PASSWORD = "SSID_PASSWORD";

const char *VENDOR_USERS_SERVER_HOSTNAME = "192.168.1.103:7075";
const char *VENDOR_USERS_SERVER_ADDRESS = "192.168.1.103";
const int VENDOR_USERS_SERVER_PORT = 7075;

WiFiClientSecure client;
std::string apiToken;

const std::string WHITESPACE = " \n\r\t\f\v";

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

bool connectToServer()
{
  if (!client.connect(VENDOR_USERS_SERVER_ADDRESS, VENDOR_USERS_SERVER_PORT))
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

void login(char *username, char *password)
{
  if(!connectToServer())
    return;
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
  apiToken = responseDoc["result"].as<std::string>();

  closeConnection();
}

void setup()
{
  Serial.begin(115200);

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

  login("admin", "admin");
  Serial.println(apiToken.c_str());
}

void loop()
{
}
