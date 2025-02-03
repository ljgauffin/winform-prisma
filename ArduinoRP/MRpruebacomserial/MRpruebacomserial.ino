#include <Control_Surface.h>

#include <NeoPixelConnect.h>

#include <EEPROM.h>


#define PRISMA_VERSION "PRISMA Midi v1.5.6"

#define EEPROM_SIZE 512

NeoPixelConnect led(16, 1, pio0, 0);


// Configuración de la librería Control Surface
USBMIDI_Interface midi;





// Pines de los botones
const pin_t buttonPins[] = { 2, 3, 4 };  // Cambiar según los pines reales
const size_t buttonCount = sizeof(buttonPins) / sizeof(buttonPins[0]);

// Variables para almacenar el modo de cada botón
enum ButtonMode { Momentary,
                  Latched };
ButtonMode buttonModes[buttonCount];

// Crear objetos de botones dinámicamente
CCButton *buttons[buttonCount] = { nullptr };
CCButtonLatched *latchedButtons[buttonCount] = { nullptr };

struct Config {
  char version[20];               // Versión del firmware
  char buttonModes[buttonCount];  // Modos de los botones ('M' o 'L')
};

// Función para configurar los botones
void setupButtons() {
  for (size_t i = 0; i < buttonCount; i++) {
    // Liberar memoria previa (si existe)
    delete buttons[i];
    delete latchedButtons[i];
    buttons[i] = nullptr;
    latchedButtons[i] = nullptr;
    Serial.print("Configurando boton ");
    Serial.print(i);
    // Crear el botón según el modo
    if (buttonModes[i] == Momentary) {
      Serial.println("MOMENTARY ");

      buttons[i] = new CCButton(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
    } else if (buttonModes[i] == Latched) {
      Serial.println("LATCHED ");

      latchedButtons[i] = new CCButtonLatched(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
    }
  }
}

void setup() {
  Control_Surface.begin();

  Serial.begin(9600);
  delay(1000);  // Dar tiempo al Serial para inicializarse
  Serial.println("Iniciando...");

  EEPROM.begin(EEPROM_SIZE);


  // El resto de tu setup...
  loadConfig();
  setupButtons();
}

void loop() {

  // Procesar comandos seriales

  if (Serial.available()) {
    String input = Serial.readStringUntil('\n');
    input.trim();

    blinkLed();
    if (input == "WHO_ARE_YOU?") {
      sendConfiguration();  //Identificación
    } else if (input == "READ") {

      readConfiguration();

    } else if (input == "LOAD") {

      loadConfig();
      setupButtons();

    } else if (input == "CLEAR") {

      clearEEPROM();

    } else {
      processCommand(input);
    }

    Serial.println(input);
  }
  // Llamada al loop de Control Surface

  Control_Surface.loop();
}

// Procesar comandos recibidos por serial
void processCommand(const String &command) {
  Config config = getConfigFromEeprom();

  if (command.length() != buttonCount) {
    Serial.println("Error: El string de modos no coincide con el número de botones.");
    return;
  }

  // Actualizar los modos de los botones
  for (size_t i = 0; i < buttonCount; i++) {
    char mode = command.charAt(i);
    if (mode == 'M' || mode == 'L') {
      config.buttonModes[i] = mode;
      Serial.print("Botón ");
      Serial.print(i);
      Serial.print(" configurado como ");
      Serial.println(mode == 'M' ? "Momentary." : "Latched.");
    } else {
      Serial.println("Error: Modo no válido en el string de modos.");
      return;
    }
  }

  // Guardar la nueva configuración en la EEPROM
  Serial.print("Config a guardar: ");
  Serial.print(config.buttonModes[0]);
  Serial.println(config.buttonModes[1]);

  writeToConfig(config);
  Serial.println("Configuración guardada en EEPROM.");


  readConfiguration();

  // Cargar la configuración actualizada

  Serial.println("Se va a levantar en los objetos.");

  loadConfig();
  Serial.println("PObjetos guardados.");
  readConfiguration();

  Serial.println("A recargar botones.");


  // Reconfigurar los botones
  setupButtons();
}



void blinkLed() {
  led.neoPixelFill(255, 0, 0, true);  // le pasamos el valor de RGB del color que queremos
  delay(500);
  led.neoPixelClear(true);
}


void clearEEPROM() {
  for (size_t i = 0; i < EEPROM_SIZE; i++) {
    EEPROM.write(i, 0);  // Escribir 0 en todas las posiciones
  }
  EEPROM.commit();  // Confirmar los cambios
  Serial.println("EEPROM limpiada correctamente.");
}

void loadConfig() {
  Config config = getConfigFromEeprom();


  if (strcmp(config.version, PRISMA_VERSION) == 0) {
    Serial.println("Configuración cargada desde EEPROM.");
    for (size_t i = 0; i < buttonCount; i++) {
      buttonModes[i] = (config.buttonModes[i] == 'M') ? Momentary : Latched;
    }
  } else {
    Serial.println("Configuración no válida. Usando valores por defecto.");
    for (size_t i = 0; i < buttonCount; i++) {
      buttonModes[i] = Momentary;
    }
    saveObjectsToConfig();
  }
}


void readConfiguration() {
  Config config = getConfigFromEeprom();

  Serial.print("Versión desde EEPROM: ");
  Serial.println(config.version);

  Serial.print("Versión desde variable: ");
  Serial.println(PRISMA_VERSION);



  // Verificar si la configuración es válida
  if (strcmp(config.version, PRISMA_VERSION) == 0) {
    Serial.println("Configuración leída desde EEPROM:");
    Serial.print("Versión: ");
    Serial.println(config.version);

    Serial.println("Modos de los botones:");
    for (size_t i = 0; i < buttonCount; i++) {
      Serial.print("Botón ");
      Serial.print(i);
      Serial.print(": ");
      Serial.println(config.buttonModes[i] == 'M' ? "Momentary" : "Latched");
    }



  } else {
    Serial.println("Configuración no válida en EEPROM.");
  }

  Serial.println("Config leida de objetos propios");


  sendConfiguration();
}

void sendConfiguration() {


  Serial.print(PRISMA_VERSION);  // Versión
  Serial.print(";");
  for (size_t i = 0; i < buttonCount; i++) {
    Serial.print(buttonModes[i] == Momentary ? 'M' : 'L');
    if (i < buttonCount - 1) {
      // Serial.print(",");  // Separador entre modos
    }
  }
  Serial.println();  // Fin de la línea
}

void saveObjectsToConfig() {
  Config config = getConfigFromEeprom();

  for (size_t i = 0; i < buttonCount; i++) {
    config.buttonModes[i] = (buttonModes[i] == Momentary) ? 'M' : 'L';
  }

  writeToConfig(config);
}

void writeToConfig(Config &config) {
  strcpy(config.version, PRISMA_VERSION);
  EEPROM.put(0, config);
  EEPROM.commit();
  Serial.println("Configuración guardada en EEPROM: ");
}

Config getConfigFromEeprom() {

  Config config;
  EEPROM.get(0, config);
  return config;
}
