#include "Arduino.h"
#include "connector/connector.h"

Connector connector;

void setup()
{
    Serial.begin(115200);
    connector.begin();
    connector.dropVending("Trakiq1", "2");
}

void loop()
{
}