// #include <Control_Surface.h>

// // Configuración de la librería Control Surface
// USBMIDI_Interface midi;



// // Pines de los botones
// const pin_t buttonPins[] = {2, 3};  // Cambiar según los pines reales
// const size_t buttonCount = sizeof(buttonPins) / sizeof(buttonPins[0]);

// // Variables para almacenar el modo de cada botón
// enum ButtonMode { Momentary, Latched };
// ButtonMode buttonModes[buttonCount];

// // Crear objetos de botones dinámicamente
// CCButton *buttons[buttonCount] = {nullptr};
// CCButtonLatched *latchedButtons[buttonCount] = {nullptr};

// // Función para configurar los botones
// void setupButtons() {
//     for (size_t i = 0; i < buttonCount; i++) {
//         // Liberar memoria previa (si existe)
//         delete buttons[i];
//         delete latchedButtons[i];
//         buttons[i] = nullptr;
//         latchedButtons[i] = nullptr;

//         // Crear el botón según el modo
//         if (buttonModes[i] == Momentary) {
//             buttons[i] = new CCButton(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
//         } else if (buttonModes[i] == Latched) {
//             latchedButtons[i] = new CCButtonLatched(buttonPins[i], MIDIAddress((int)MIDI_CC::General_Purpose_Controller_1 + i, Channel_1));
//         }
//     }
// }

// void setup() {
//   pinMode(LED_PIN, OUTPUT);
// digitalWrite(LED_PIN, HIGH); 
//     Serial.begin(9600);
//     Serial.println("Listo para recibir configuraciones.");

//     // Configuración inicial de los botones
//     for (size_t i = 0; i < buttonCount; i++) {
//         buttonModes[i] = Momentary;  // Configuración inicial como Momentary
//     }
//     setupButtons();

//     // Iniciar Control Surface
//     Control_Surface.begin();
// }

// void loop() {
//     // Llamada al loop de Control Surface
//     Control_Surface.loop();

//     // Procesar comandos seriales
//     if (Serial.available()) {
//         String input = Serial.readStringUntil('\n');  // Leer comando desde serial
//    input.trim(); // Elimina los espacios en blanco directamente
// processCommand(input); // Envía el String procesado a la función
//     }
// }

// // Procesar comandos recibidos por serial
// void processCommand(const String &command) {

//     if (command.startsWith("SET")) {
//         int buttonIndex = command.substring(4, 5).toInt();  // Extraer índice del botón
//         char mode = command.charAt(6);                     // Extraer el modo

//         if (buttonIndex >= 0 && buttonIndex < buttonCount) {
//             if (mode == 'M') {
//                 buttonModes[buttonIndex] = Momentary;
//                 Serial.print("Botón ");
//                 Serial.print(buttonIndex);
//                 Serial.println(" configurado como Momentary.");
//             } else if (mode == 'L') {
//                 buttonModes[buttonIndex] = Latched;
//                 Serial.print("Botón ");
//                 Serial.print(buttonIndex);
//                 Serial.println(" configurado como Latched.");
//             } else {
//                 Serial.println("Modo inválido. Use 'M' o 'L'.");
//             }
//             setupButtons();  // Reconfigurar botones
//         } else {
//             Serial.println("Índice de botón inválido.");
//         }
//     } else {
//         Serial.println("Comando no reconocido. Use 'SET <índice> <M/L>'.");
//     }
// }



#include <NeoPixelConnect.h>

NeoPixelConnect led(16, 1, pio0, 0);

void setup(){

      Serial.begin(9600);
    Serial.println("Listo para recibir configuraciones.");

}


void loop(){

    if (Serial.available()) {
        String input = Serial.readStringUntil('\n');  
        input.trim(); 
        if (input == "WHO_ARE_YOU?") {
            Serial.println("Prisma Expression pedal A1 v1.2.1");  // Respuesta de identificación
        }
        else {
            processCommand(input);
        }
    }


}



void processCommand(const String &command) {

    if (command.startsWith("A")) {

          Serial.println("ROJO");

          led.neoPixelFill(255, 0, 0, true); // le pasamos el valor de RGB del color que queremos
    delay(1000); 
    led.neoPixelClear(true); // limpiamos el led
    } else if (command.startsWith("B")) {

                Serial.println("AZUL");

    led.neoPixelFill(0, 0, 255, true); // le pasamos el valor de RGB del color que queremos
    delay(1000);
    led.neoPixelClear(true); // limpiamos el led
    }
}
