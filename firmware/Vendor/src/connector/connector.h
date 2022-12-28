#ifndef CONNECTOR_H
#define CONNECTOR_H

#include "Arduino.h"
#include "SIM800L.h"
#include <string>

#define SIM800_RST_PIN 5
#define W_TIMEOUT 100000

const char JSON_CTYPE[] = "application/json";

const char APN[] = "a1.internet.bg";
const String HOST = "http://91.139.199.150:8080";

class Connector
{
private:
    SIM800L *sim800l;
    bool gprsConnected = false;
    bool connectGprs();
    bool disconnectGprs();

public:
    bool begin();
    int dropVending(std::string vendingTitle, std::string spiral);
};

#endif