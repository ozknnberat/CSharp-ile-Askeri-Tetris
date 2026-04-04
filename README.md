# 🎖️ Askeri Tetris - Turkiye Yapimi

Klasik Tetris oyununun Türk Komando temasıyla yeniden tasarlanmış hali.
C# ve MonoGame kullanılarak geliştirilmiştir.

---

## 🎮 Oyun Hakkında

Bu oyun, klasik Tetris mekanigini Turk askeri temasıyla birlestiren
ozgun bir oyun senaryosudur. Dusen bloklar farkli askeri birlikleri
temsil etmekte olup her biri Turk Komando kamuflaj deseniyle
tasarlanmistir.

### Askeri Birlikler (Bloklar)

| Blok | Birlik       |
|------|--------------|
| I    | Hendek       |
| L    | Top Mevzii   |
| J    | Barikat      |
| O    | Kule         |
| S    | Mayin        |
| Z    | Tel Orgu     |
| T    | Komuta       |

---

## 🕹️ Kontroller

| Tus         | Islem              |
|-------------|--------------------|
| ← →         | Saga / Sola Hareket|
| ↑           | Bloku Dondur       |
| ↓           | Hizli Dusur        |
| R           | Yeniden Baslat     |

---

## 🏆 Puanlama

- Her doldurulan satir: **+100 puan**
- Satir temizlendikce oyun hizlanir
- Ne kadar cok satir temizlersen seviye o kadar yukselir

---

## 🔊 Ses Efektleri

| Olay                  | Ses           |
|-----------------------|---------------|
| Oyun baslangici       | Ping          |
| Blok yerlesince       | Tink          |
| Satir temizlenince    | Glass (Ates)  |
| Oyun bitince          | Basso (Bomba) |
| Donus / Hareket       | Pop           |

---

## ⚙️ Kurulum

### Gereksinimler
- .NET 8 veya uzeri
- MonoGame 3.8.4

### Adimlar

1. Repoyu klonla veya dosyaları indir
2. Terminal / Komut Satiri ac
3. Proje klasorune gir:
4. Calistir:

---

## 📁 Dosya Yapisi
TetrisSim/
├── Game1.cs          ← Ana oyun kodu
├── Program.cs        ← Oyunu baslatan dosya
├── TetrisSim.csproj  ← Proje dosyasi
├── README.md         ← Bu dosya
└── Content/
├── Content.mgcb  ← Icerik yapılandirmasi
└── Font.spritefont ← Yazi tipi

---

## 🇹🇷 Sag Panelde Kayan Yazi

Oyunun sag panelinde Mustafa Kemal Ataturk'un
**Genclere Hitabesi** yukari dogru kaymaktadir.

---

> "Muhtac oldugun kudret, damarlarindaki asil kanda mevcuttur!"
> Mustafa Kemal Ataturk