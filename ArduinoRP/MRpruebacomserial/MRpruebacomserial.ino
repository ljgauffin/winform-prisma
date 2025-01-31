#include <Control_Surface.h>

#include <NeoPixelConnect.h>

#include <LittleFS.h>

#define CONFIG_FILE "/config.txt"

#define DEFAULT_CONFIG "PRISMA Midi controller v1.5.6;MLLMLM"

NeoPixelConnect led(16, 1, pio0, 0);


// Configuración de la librería Control Surface
USBMIDI_Interface midi;



// Pines de los botones
const pin_t buttonPins[] = { 2, 3 };  // Cambiar según los pines reales
const size_t buttonCount = sizeof(buttonPins) / sizeof(buttonPins[0]);

// Variables para almacenar el modo de cada botón
enum ButtonMode { Momentary,
                  Latched };
ButtonMode buttonModes[buttonCount];

// Crear objetos de botones dinámicamente
CCButton *buttons[buttonCount] = { nullptr };
CCButtonLatched *latchedButtons[buttonCount] = { nullptr };

// Función para configurar los botones
void setupButtons() {
  for (size_t i = 0; i < buttonCount; i++) {
    // Liberar memoria previa (si existe)
    delete buttons[i];
    delete latchedButtons[i];
    buttons[i] = nullptr;
    latchedButtons[i] = nullptr;

    // Crear el botón según el modo
    if (buttonModes[i] == Momentary) {
      buttons[i] = new CCButton(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
    } else if (buttonModes[i] == Latched) {
      latchedButtons[i] = new CCButtonLatched(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
    }
  }
}

void setup() {

  loadConfig();

  Serial.begin(9600);
  Serial.println("Listo para recibir configuraciones.");

  // Configuración inicial de los botones
  for (size_t i = 0; i < buttonCount; i++) {
    buttonModes[i] = Momentary;  // Configuración inicial como Momentary
  }
  setupButtons();

  // Iniciar Control Surface
  Control_Surface.begin();
}

void loop() {

  // Procesar comandos seriales

  if (Serial.available()) {
    String input = Serial.readStringUntil('\n');
    input.trim();

    blinkLed();
    if (input == "WHO_ARE_YOU?") {
      Serial.println("Prisma Expression pedal A1 v1.2.1");  // Respuesta de identificación
    } else if (input == "READ") {

      readConfiguration();

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

  if (command.startsWith("SET")) {
    int buttonIndex = command.substring(4, 5).toInt();  // Extraer índice del botón
    char mode = command.charAt(6);                      // Extraer el modo

    if (buttonIndex >= 0 && buttonIndex < buttonCount) {
      blinkLed();
      if (mode == 'M') {

        buttonModes[buttonIndex] = Momentary;
        Serial.print("Botón ");
        Serial.print(buttonIndex);
        Serial.println(" configurado como Momentary.");
      } else if (mode == 'L') {
        buttonModes[buttonIndex] = Latched;
        Serial.print("Botón ");
        Serial.print(buttonIndex);
        Serial.println(" configurado como Latched.");
      } else {
        Serial.println("Modo inválido. Use 'M' o 'L'.");
      }
      setupButtons();  // Reconfigurar botones
    } else {
      Serial.println("Índice de botón inválido.");
    }
  } else {
    Serial.println("Comando no reconocido. Use 'SET <índice> <M/L>'.");
  }
}



void blinkLed() {
  led.neoPixelFill(255, 0, 0, true);  // le pasamos el valor de RGB del color que queremos
  delay(500);
  led.neoPixelClear(true);
}

void loadConfig() {
  LittleFS.format();

  if (!LittleFS.begin()) {
    Serial.println("Error: No se pudo inicializar LittleFS.");
    return;
  }

  if (!LittleFS.exists(CONFIG_FILE)) {
    Serial.println("No existe configuración. Creando archivo...");
    saveDefaultConfig();
  }

  File file = LittleFS.open(CONFIG_FILE, "r");
  if (!file) {
    Serial.println("Error al abrir el archivo después de crearlo.");
    return;
  }

  Serial.println("Archivo abierto correctamente.");
  file.close();

}

void saveDefaultConfig() {
  File file = LittleFS.open(CONFIG_FILE, "w");
  if (!file) {
    Serial.println("Error al crear el archivo de configuración.");
    return;
  }

  file.write(DEFAULT_CONFIG, strlen(DEFAULT_CONFIG));  // Guardar la configuración
  file.close();
  Serial.println("Archivo de configuración guardado.");
}

void readConfiguration() {
 
  File file = LittleFS.open(CONFIG_FILE, "r");  // Abrir en modo lectura
  if (!file) {
    Serial.println("Error al abrir el archivo.");
    return;
  }

  Serial.println("Contenido del archivo:");
  while (file.available()) {
    Serial.write(file.read());  // Leer y enviar cada byte al Serial
  }
  Serial.println("\n--- Fin del archivo ---");
  file.close();
}

