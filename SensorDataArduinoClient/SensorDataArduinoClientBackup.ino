#include <FS.h>
#include <ESP8266WiFi.h>
#include <DNSServer.h>
#include <WiFiManager.h>
#include <ESP8266WebServer.h>
#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>


String configFileName = "/config.json";

char owner[40];
char serviceAddress[40];
char servicePort[6];
long deviceId;
String server;
String port;

WiFiManagerParameter customOwner("owner", "Owner key", owner, 40);
WiFiManagerParameter customServiceAddress("server", "Server address", serviceAddress, 40);
WiFiManagerParameter customServicePort("port", "Server port", servicePort, 6);

bool shouldSaveConfig = false;

void writeSettings() {
  strcpy(owner, customOwner.getValue());
  strcpy(serviceAddress, customServiceAddress.getValue());
  strcpy(servicePort, customServicePort.getValue());

  Serial.println("saving config..");
  DynamicJsonBuffer jsonBuffer;
  JsonObject& json = jsonBuffer.createObject();
  json["owner"] = owner;
  json["server"] = serviceAddress;
  json["port"] = servicePort;

  SPIFFS.begin();
  File configFile = SPIFFS.open(configFileName, "w");
  if (!configFile) {
    Serial.println("failed to open config file for writing");
  } else {
    json.printTo(Serial);
    json.printTo(configFile);
  }
  configFile.close();
}


void runWiFiManager() {
  WiFiManager wifiManager;
  wifiManager.setSaveConfigCallback(onSaveConfig);

  wifiManager.addParameter(&customOwner);
  wifiManager.addParameter(&customServiceAddress);
  wifiManager.addParameter(&customServicePort);

  //Remove later
  //wifiManager.resetSettings();

  if (!wifiManager.autoConnect("SensorDataClient", "connectM3")) {
    delay(3000);
    ESP.reset();
    delay(5000);
  }
}

void onSaveConfig() {
  shouldSaveConfig = true;
}

String getConfigValue(String name) {
  String value = "";
  if (SPIFFS.begin()) {
    if (SPIFFS.exists(configFileName)) {
      File configFile = SPIFFS.open(configFileName, "r");
      if (configFile) {
        size_t size = configFile.size();
        std::unique_ptr<char[]> buf(new char [size]);

        configFile.readBytes(buf.get(), size);
        DynamicJsonBuffer jsonBuffer;
        JsonObject& json = jsonBuffer.parseObject(buf.get());
        if (json.success()) {
          return json[name];
        }
      }
    }
  }
  return value;
}



void setup() {
  Serial.begin(115200);
  delay(500);

  deviceId = ESP.getChipId();

  runWiFiManager();
  Serial.println("Connected!");

  if (shouldSaveConfig) {
    writeSettings();
  }

  server = getConfigValue("server");
  port = getConfigValue("port");

  Serial.println("local ip");
  Serial.println(WiFi.localIP());

}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin("http://" + server + ":" + port + "/api/devices");      //Specify request destination
    Serial.println("http://" + server + ":" + port + "/api/devices");
    http.addHeader("Content-Type", "application/json");  //Specify content-type header
    String postData = "{";
    postData += "\"deviceId\" : \"" + (String) deviceId + "\",";
    postData += "\"sensorData\" : [";
    postData += "{ \"sensorKey\" : \"HumiditySensor\"";
    postData += ",\"sensorType\":" + (String) 1;
    postData += ",\"value\" : " + (String) random(20, 80);
    postData += "}";
    postData += "]";
    postData += "}";
    Serial.println(postData);
    int httpCode = http.POST(postData);   //Send the request
    String payload = http.getString();                  //Get the response payload

    Serial.println(httpCode);   //Print HTTP return code
    Serial.println(payload);    //Print request response payload

    http.end();  //Close connection
  }

  delay(100000);
}



