bool state7 = 0;
bool state8 = 0;
bool state9 = 0;
void setup()
{
  Serial.begin(9600);

  DDRB = 0b00000000;
  DDRD = 0b00000000;
}
void loop() {
  state7 = (PIND>>7) & 0b00000001;
  state8 = PINB & 0b00000001;
  state9 = (PINB>>1) & 0b00000001;
  Serial.println(((state7<<3)|(state8<<2)|(state9<<1)|0));
  delay(10);
}