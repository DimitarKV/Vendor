#include "Arduino.h"
#include <string>
#include "connector.h"

bool Connector::begin()
{
    Serial2.begin(115200);
    this->sim800l = new SIM800L((Stream *)&Serial2, SIM800_RST_PIN, 16384, 16384);

    while (!this->sim800l->isReady())
    {
        Serial.println("AT not yet ready!");
        delay(50);
    }
    Serial.println("AT ready!");

    uint8_t signal = this->sim800l->getSignal();
    while (signal <= 0)
    {
        // delay(50);
        signal = this->sim800l->getSignal();
    }
    Serial.println("Having signal!");
    // delay(1000);

    NetworkRegistration network = this->sim800l->getRegistrationStatus();
    while (network != REGISTERED_HOME && network != REGISTERED_ROAMING)
    {
        // delay(50);
        network = this->sim800l->getRegistrationStatus();
    }
    Serial.println("Registered network");
    // delay(50);

    // Setup APN for GPRS configuration
    bool success = this->sim800l->setupGPRS(APN);
    while (!success)
    {
        success = this->sim800l->setupGPRS(APN);
        delay(50);
    }
    Serial.println("GPRS ready!");
    return true;
}

int Connector::dropVending(std::string vendingTitle, std::string spiral)
{
    this->connectGprs();

    uint16_t rc = this->sim800l->doPost((HOST + "/vending/drop").c_str(), 
    JSON_CTYPE, "{\"title\": \"Trakiq1\",\"spiral\": \"2\"}", W_TIMEOUT, W_TIMEOUT);
    Serial.println(rc);

    if (rc != 200)
    {
        this->disconnectGprs();
        return rc;
    }

    Serial.println(this->sim800l->getDataReceived());

    this->disconnectGprs();
}

bool Connector::connectGprs()
{
    for (uint8_t i = 0; i < 5 && !gprsConnected; i++)
    {
        delay(1000);
        gprsConnected = this->sim800l->connectGPRS();
    }

    if (!gprsConnected)
        return false;
    Serial.println("GPRS connected!");
    return true;
}

bool Connector::disconnectGprs()
{
    gprsConnected = !this->sim800l->disconnectGPRS();
    for (uint8_t i = 0; i < 5 && gprsConnected; i++)
    {
        gprsConnected = !this->sim800l->disconnectGPRS();
    }

    if (gprsConnected)
        false;
    Serial.println("GPRS disconnected!");
    return true;
}