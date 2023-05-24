#include <WiFiClientSecure.h>

const char* ssid = "SUPERGIRL-2";
const char* password = "20042007pushkin";

const char*  server = "192.168.1.104";  // Server URL

// www.howsmyssl.com root certificate authority, to verify the server
// change it to your server root CA
// SHA1 fingerprint is broken now!

const char* test_root_ca=
"-----BEGIN CERTIFICATE-----\n"
"MIIDDDCCAfSgAwIBAgIIcoWyKLckFlMwDQYJKoZIhvcNAQELBQAwFDESMBAGA1UE\n"
"AxMJbG9jYWxob3N0MB4XDTIzMDQyNDE5MzU1OFoXDTI0MDQyMzE5MzU1OFowFDES\n"
"MBAGA1UEAxMJbG9jYWxob3N0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKC\n"
"AQEAwznIuaBPzXaf5/QJtnUgV4NI3AOP6TqnCMSr/JfH0Ll+ONjSl5uFM548I0Wm\n"
"enAd6uPfFTlpAtAkfzFJ+kL//pksvz2JoP0HtewR534t8KfuS2Uc01XLyTB8+e4N\n"
"PiFldxQCduD6RFXZWcSOMOd/9rk7yEtgGVWysNMTNO+zSm+oXKYuDYDqN6g/Njcz\n"
"Z4whyPgZT2bi+3fGt17psVRMxBQ5DJt0FQwbPkVlNhCh8lUYHJcFDXiL3HI8D3lv\n"
"Q7NpL1UZKetEc146Gw78DYpEfK+EGhhbVEjqcDDcuQv9jxF/9ZYknJYYvXxu63oi\n"
"TBUkCTGBAn4W9tSG64etdOQACQIDAQABo2IwYDAMBgNVHRMBAf8EAjAAMA4GA1Ud\n"
"DwEB/wQEAwIFoDAWBgNVHSUBAf8EDDAKBggrBgEFBQcDATAXBgNVHREBAf8EDTAL\n"
"gglsb2NhbGhvc3QwDwYKKwYBBAGCN1QBAQQBAjANBgkqhkiG9w0BAQsFAAOCAQEA\n"
"ElD5JNdzW3VEKgBnnOAGLSGnc1aOeAeQ4ietITRBkUVjh9FY4B9o47iizCYFuSCK\n"
"98tq7b0NzkMaCmmM/4Dl5mYcxUZSEqZi5gx0pPw3gt/SNMKAqpk31AnOHCxlTC/D\n"
"ak5reCG0Bb5wgugIeL0+vDmZc2JomNTMeNO2oAsdlYoYJNHoEPDgUzyL+RTHXVk0\n"
"8wTM28CyfW/GvDM5Dg7+NgabdL5ly0gSE+HSHkVd33BwFxEX7yvY29+od6XNZ5dq\n"
"jZ67I8uPZz48/SJbhcPiLwIApmkogRNahnDOLmnIEAogyG80XRg/tG1MdVoPGYmY\n"
"qoEE9QgIucaXergT0oKY8w==\n"
"-----END CERTIFICATE-----"; 

// You can use x.509 client certificates if you want
//const char* test_client_key = "";   //to verify the client
//const char* test_client_cert = "";  //to verify the client


WiFiClientSecure client;

void setup() {
  //Initialize serial and wait for port to open:
  Serial.begin(115200);
  delay(100);

  Serial.print("Attempting to connect to SSID: ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);  
  WiFi.begin(ssid, password);

  // attempt to connect to Wifi network:
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    // wait 1 second for re-trying
    delay(1000);
  }

  Serial.print("Connected to ");
  Serial.println(ssid);

  client.setCACert(test_root_ca);
  //client.setCertificate(test_client_cert); // for client verification
  //client.setPrivateKey(test_client_key);  // for client verification
  delay(2000);

  Serial.println("\nStarting connection to server...");
  if (!client.connect(server, 7075))
    Serial.println("Connection failed!");
  else {
    Serial.println("Connected to server!");
    // Make a HTTP request:
    client.println("POST /user/login HTTP/1.1");
    client.println("Host: 192.168.1.104:7075");
    client.println("Content-Type: application/json");
    client.println("Content-Length: 44");
    client.println();
    client.println("{ \"username\": \"admin\", \"password\": \"admin\" }");


    while (client.connected()) {
      String line = client.readStringUntil('\n');
      if (line == "\r") {
        Serial.println("headers received");
        break;
      }
    }
    // if there are incoming bytes available
    // from the server, read them and print them:
    while (client.available()) {
      char c = client.read();
      Serial.write(c);
    }

    client.stop();
  }
}

void loop() {
  // do nothing
}
