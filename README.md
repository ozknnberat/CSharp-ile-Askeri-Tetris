# 🎖️ C# ile Askeri Tetris

Klasik Tetris oyununun Turk Komando temasıyla yeniden tasarlanmis hali.
C# ve MonoGame kullanılarak gelistirilmistir.

---

## 🎮 Oyun Hakkında
Klasik Tetris mekanigini Turk askeri temasıyla birlestiren ozgun bir oyundur.
Dusen bloklar farkli askeri birlikleri temsil eder.
Sag panelde Ataturk'un Genclere Hitabesi akar.

---

## ⚙️ Kurulum (Windows)

### 1. .NET yukle
https://dotnet.microsoft.com/download adresine git, .NET 9 indir ve kur.

### 2. Git yukle
https://git-scm.com/download/win adresine git ve kur.

### 3. Repoyu indir
Komut Satiri (CMD) ac ve yaz:
git clone https://github.com/ozknnberat/C-ile-Askeri-Tetris.git
cd C-ile-Askeri-Tetris
dotnet run

---

## 🕹️ Kontroller

| Tus         | Islem               |
|-------------|---------------------|
| ← →         | Saga / Sola Hareket |
| ↑           | Bloku Dondur        |
| ↓           | Hizli Dusur         |
| R           | Yeniden Baslat      |

---

## 🏆 Puanlama
- Her doldurulan satir: +100 puan
- Satir temizlendikce oyun hizlanir

---

## 🔊 Ses Efektleri
- Oyun baslangici: Ping
- Blok yerlesince: Tink
- Satir temizlenince: Glass
- Oyun bitince: Basso (Bomba)
- Donus/Hareket: Pop

---

> "Muhtaç olduğun kudret, damarlarındaki asil kanda mevcuttur!"
> — Mustafa Kemal Atatürk
