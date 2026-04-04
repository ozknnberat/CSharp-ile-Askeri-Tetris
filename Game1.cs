using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D kutu;
    private SpriteFont font;

    const int SATIR = 20;
    const int SUTUN = 10;
    const int HUCRE = 32;
    const int SOL_PANEL = 180;
    const int SAG_PANEL = 220;

    int[,] tahta = new int[SATIR, SUTUN];
    int[,] tahtaRenk = new int[SATIR, SUTUN];

    int[][,] tumBloklar = {
        new int[,] { {1,1,1,1} },
        new int[,] { {1,0},{1,0},{1,1} },
        new int[,] { {0,1},{0,1},{1,1} },
        new int[,] { {1,1},{1,1} },
        new int[,] { {0,1,1},{1,1,0} },
        new int[,] { {1,1,0},{0,1,1} },
        new int[,] { {1,1,1},{0,1,0} },
    };

    Color[][] kamuflaj = {
        new Color[]{ new Color(34,85,34),  new Color(60,120,40), new Color(80,60,20)  },
        new Color[]{ new Color(90,70,20),  new Color(120,100,30),new Color(60,90,30)  },
        new Color[]{ new Color(40,70,100), new Color(30,50,80),  new Color(60,80,50)  },
        new Color[]{ new Color(60,80,20),  new Color(40,60,10),  new Color(80,70,30)  },
        new Color[]{ new Color(100,30,20), new Color(80,20,10),  new Color(60,40,20)  },
        new Color[]{ new Color(50,70,30),  new Color(70,90,40),  new Color(40,50,20)  },
        new Color[]{ new Color(80,50,20),  new Color(60,40,15),  new Color(90,70,30)  },
    };

    string[] blokIsimleri = { "Hendek","Top Mevzii","Barikat","Kule","Mayin","Tel Orgu","Komuta" };

    string[] hitabe = {
        "Ataturk'un",
        "Genclere Hitabesi",
        "",
        "Ey Turk gencligi!",
        "Birinci vazifen;",
        "Turk istiklalini,",
        "Turk cumhuriyetini,",
        "ilelebet muhafaza",
        "ve mudafaa etmektir.",
        "Mevcudiyetinin",
        "ve istikbalinin",
        "yegane temeli budur.",
        "Bu temel, senin",
        "en kiymetli hazinendir.",
        "Istikbalde dahi seni",
        "bu hazineden mahrum",
        "etmek isteyecek",
        "dahili ve harici",
        "bedhahların olacaktır.",
        "Bir gun, istiklal",
        "ve cumhuriyeti",
        "mudafaa mecburiyetine",
        "dusersen, vazifeye",
        "atilmak icin icinde",
        "bulunacagin vaziyetin",
        "imkan ve seraitini",
        "dusunmeyeceksin.",
        "Istiklal ve",
        "cumhuriyetine",
        "kastedecek dusmanlar,",
        "butun dunyada emsali",
        "gorulmemis bir",
        "galibiyetin",
        "mumessili olabilirler.",
        "Cebren ve hile ile",
        "aziz vatanin butun",
        "kaleleri zapt edilmis,",
        "butun tersanelerine",
        "girilmis, butun",
        "ordulari dagitilmis",
        "ve memleketin her",
        "kosesi bilfiil",
        "isgal edilmis olabilir.",
        "",
        "Ey Turk istikbalinin",
        "evladi! Iste, bu",
        "ahval icinde dahi",
        "vazifen, Turk istiklal",
        "ve cumhuriyetini",
        "kurtarmaktir.",
        "Muhtac oldugun kudret,",
        "damarlarindaki",
        "asil kanda mevcuttur!",
        "",
        "- Mustafa Kemal",
        "  Ataturk",
        "",
        "",
    };

    float kaymaY;
    float kaymaHizi = 30f;

    int[,] aktifBlok;
    int aktifTip;
    int blokSatir, blokSutun;

    float zamanlayici = 0f;
    float hiz = 0.7f;

    int puan = 0;
    int satirSayaci = 0;
    bool oyunBitti = false;
    bool hosgeldinEkrani = true;

    bool bravoGoster = false;
    float bravoSure = 0f;

    Random rnd = new Random();
    KeyboardState eskiKlavye;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth  = SOL_PANEL + SUTUN * HUCRE + SAG_PANEL;
        _graphics.PreferredBackBufferHeight = SATIR * HUCRE;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    void Ses(string tip)
    {
        try
        {
            string dosya = tip switch
            {
                "blok"  => "/System/Library/Sounds/Tink.aiff",
                "ates"  => "/System/Library/Sounds/Glass.aiff",
                "bomba" => "/System/Library/Sounds/Basso.aiff",
                "basla" => "/System/Library/Sounds/Ping.aiff",
                "donus" => "/System/Library/Sounds/Pop.aiff",
                _       => "/System/Library/Sounds/Tink.aiff",
            };
            Process.Start(new ProcessStartInfo("afplay", dosya)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow  = true
            });
        }
        catch { }
    }

    string Temizle(string metin)
    {
        return metin
            .Replace("ı","i").Replace("İ","I")
            .Replace("ğ","g").Replace("Ğ","G")
            .Replace("ü","u").Replace("Ü","U")
            .Replace("ş","s").Replace("Ş","S")
            .Replace("ö","o").Replace("Ö","O")
            .Replace("ç","c").Replace("Ç","C")
            .Replace("â","a").Replace("î","i")
            .Replace("û","u").Replace("é","e")
            .Replace("'","'").Replace("'","'")
            .Replace("\u0435","e");
    }

    protected override void Initialize()
    {
        kaymaY = SATIR * HUCRE;
        YeniBlok();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        kutu = new Texture2D(GraphicsDevice, 1, 1);
        kutu.SetData(new Color[] { Color.White });
        try { font = Content.Load<SpriteFont>("Font"); } catch { font = null; }
    }

    void YeniBlok()
    {
        aktifTip  = rnd.Next(0, tumBloklar.Length);
        aktifBlok = tumBloklar[aktifTip];
        blokSatir = 0;
        blokSutun = SUTUN / 2 - aktifBlok.GetLength(1) / 2;
        if (!GecerliMi(aktifBlok, blokSatir, blokSutun))
        {
            oyunBitti = true;
            Ses("bomba");
        }
    }

    void OyunuSifirla()
    {
        tahta           = new int[SATIR, SUTUN];
        tahtaRenk       = new int[SATIR, SUTUN];
        puan            = 0;
        satirSayaci     = 0;
        hiz             = 0.7f;
        zamanlayici     = 0f;
        kaymaY          = SATIR * HUCRE;
        oyunBitti       = false;
        hosgeldinEkrani = false;
        bravoGoster     = false;
        YeniBlok();
        Ses("basla");
    }

    bool GecerliMi(int[,] blok, int satir, int sutun)
    {
        for (int s = 0; s < blok.GetLength(0); s++)
            for (int c = 0; c < blok.GetLength(1); c++)
                if (blok[s, c] == 1)
                {
                    int ys = satir + s;
                    int yc = sutun + c;
                    if (ys < 0 || ys >= SATIR || yc < 0 || yc >= SUTUN) return false;
                    if (tahta[ys, yc] == 1) return false;
                }
        return true;
    }

    int[,] Dondur(int[,] blok)
    {
        int s = blok.GetLength(0);
        int c = blok.GetLength(1);
        int[,] yeni = new int[c, s];
        for (int i = 0; i < s; i++)
            for (int j = 0; j < c; j++)
                yeni[j, s - 1 - i] = blok[i, j];
        return yeni;
    }

    void Yerles()
    {
        for (int s = 0; s < aktifBlok.GetLength(0); s++)
            for (int c = 0; c < aktifBlok.GetLength(1); c++)
                if (aktifBlok[s, c] == 1)
                {
                    tahta[blokSatir + s, blokSutun + c] = 1;
                    tahtaRenk[blokSatir + s, blokSutun + c] = aktifTip;
                }

        Ses("blok");

        bool satirTemizlendi = false;
        for (int s = SATIR - 1; s >= 0; s--)
        {
            bool dolu = true;
            for (int c = 0; c < SUTUN; c++)
                if (tahta[s, c] == 0) { dolu = false; break; }

            if (dolu)
            {
                puan += 100;
                satirSayaci++;
                satirTemizlendi = true;
                for (int ust = s; ust > 0; ust--)
                    for (int c = 0; c < SUTUN; c++)
                    {
                        tahta[ust, c]     = tahta[ust - 1, c];
                        tahtaRenk[ust, c] = tahtaRenk[ust - 1, c];
                    }
                s++;
            }
        }

        if (satirTemizlendi)
        {
            bravoGoster = true;
            bravoSure   = 2.0f;
            hiz = Math.Max(0.2f, hiz - 0.02f);
            Ses("ates");
        }

        YeniBlok();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState klavye = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (hosgeldinEkrani)
        {
            if (klavye.GetPressedKeys().Length > 0 && eskiKlavye.GetPressedKeys().Length == 0)
            {
                hosgeldinEkrani = false;
                Ses("basla");
            }
            eskiKlavye = klavye;
            return;
        }

        if (oyunBitti)
        {
            if (klavye.IsKeyDown(Keys.R) && !eskiKlavye.IsKeyDown(Keys.R))
                OyunuSifirla();
            eskiKlavye = klavye;
            return;
        }

        if (bravoGoster)
        {
            bravoSure -= dt;
            if (bravoSure <= 0f) bravoGoster = false;
        }

        kaymaY -= kaymaHizi * dt;
        if (kaymaY < -(hitabe.Length * 22f))
            kaymaY = SATIR * HUCRE;

        if (klavye.IsKeyDown(Keys.Left) && !eskiKlavye.IsKeyDown(Keys.Left))
            if (GecerliMi(aktifBlok, blokSatir, blokSutun - 1)) { blokSutun--; Ses("donus"); }

        if (klavye.IsKeyDown(Keys.Right) && !eskiKlavye.IsKeyDown(Keys.Right))
            if (GecerliMi(aktifBlok, blokSatir, blokSutun + 1)) { blokSutun++; Ses("donus"); }

        if (klavye.IsKeyDown(Keys.Down) && !eskiKlavye.IsKeyDown(Keys.Down))
            if (GecerliMi(aktifBlok, blokSatir + 1, blokSutun)) blokSatir++;

        if (klavye.IsKeyDown(Keys.Up) && !eskiKlavye.IsKeyDown(Keys.Up))
        {
            int[,] d = Dondur(aktifBlok);
            if (GecerliMi(d, blokSatir, blokSutun)) { aktifBlok = d; Ses("donus"); }
        }

        zamanlayici += dt;
        if (zamanlayici >= hiz)
        {
            if (GecerliMi(aktifBlok, blokSatir + 1, blokSutun)) blokSatir++;
            else Yerles();
            zamanlayici = 0f;
        }

        eskiKlavye = klavye;
        base.Update(gameTime);
    }

    Color KamuflajRenk(int tip, int satir, int sutun)
    {
        int indeks = (satir * 3 + sutun * 2 + tip) % 3;
        return kamuflaj[tip][indeks];
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 30, 15));
        _spriteBatch.Begin();

        if (hosgeldinEkrani)
        {
            Rect(0, 0, SOL_PANEL + SUTUN*HUCRE + SAG_PANEL, SATIR*HUCRE, new Color(10,20,10));
            Rect(0, SATIR*HUCRE/2-70, SOL_PANEL + SUTUN*HUCRE + SAG_PANEL, 140, new Color(180,0,0,210));
            Yazi("ASKERI TETRIS",              SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2-45, Color.White,   true);
            Yazi("Hosgeldin, Asker!",          SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2-15, Color.Yellow,  true);
            Yazi("Turkiye Yapimi",             SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2+15, new Color(200,200,200), true);
            Yazi("Baslamak icin bir tusa bas", SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2+45, Color.LightGray, true);
            _spriteBatch.End();
            return;
        }

        // Sol panel
        Rect(0, 0, SOL_PANEL, SATIR*HUCRE, new Color(15,25,10));
        Rect(5, 5, SOL_PANEL-10, 2, new Color(180,0,0));
        Yazi("ASKERI TETRIS",  10, 15, new Color(220,220,220));
        Yazi("Turkiye Yapimi", 10, 33, new Color(180,0,0));
        Rect(5, 52, SOL_PANEL-10, 2, new Color(180,0,0));
        Yazi("PUAN",     10,  62, new Color(180,180,100));
        Yazi(puan.ToString(), 10, 80, Color.White);
        Yazi("SATIRLAR", 10, 108, new Color(180,180,100));
        Yazi(satirSayaci.ToString(), 10, 126, Color.White);
        Yazi("SEVIYE",   10, 154, new Color(180,180,100));
        Yazi(((int)((0.7f - hiz) / 0.02f + 1)).ToString(), 10, 172, Color.White);
        Rect(5, 198, SOL_PANEL-10, 2, new Color(85,107,47));
        Yazi("KONTROLLER",    10, 208, new Color(180,180,100));
        Yazi("<- -> Hareket", 10, 228, Color.LightGray);
        Yazi("^  Dondur",     10, 246, Color.LightGray);
        Yazi("v  Hizlan",     10, 264, Color.LightGray);
        Yazi("R  Yeniden",    10, 282, Color.LightGray);
        Rect(5, 305, SOL_PANEL-10, 2, new Color(85,107,47));
        Yazi("BIRLIK", 10, 315, new Color(180,180,100));
        Yazi(blokIsimleri[aktifTip], 10, 333, kamuflaj[aktifTip][0]);

        // Oyun tahtasi
        for (int s = 0; s < SATIR; s++)
            for (int c = 0; c < SUTUN; c++)
            {
                Color renk = tahta[s, c] == 1
                    ? KamuflajRenk(tahtaRenk[s, c], s, c)
                    : new Color(30,42,25);
                Rect(SOL_PANEL + c*HUCRE+1, s*HUCRE+1, HUCRE-2, HUCRE-2, renk);
                if (tahta[s, c] == 1)
                {
                    int tip = tahtaRenk[s, c];
                    Color n = kamuflaj[tip][(s+c+1)%3];
                    Rect(SOL_PANEL + c*HUCRE+6,  s*HUCRE+6,  8, 8, n);
                    Rect(SOL_PANEL + c*HUCRE+18, s*HUCRE+14, 6, 6, n);
                    Rect(SOL_PANEL + c*HUCRE+10, s*HUCRE+20, 7, 5, n);
                }
            }

        // Aktif blok
        for (int s = 0; s < aktifBlok.GetLength(0); s++)
            for (int c = 0; c < aktifBlok.GetLength(1); c++)
                if (aktifBlok[s, c] == 1)
                {
                    Color ana = KamuflajRenk(aktifTip, s, c);
                    Rect(SOL_PANEL + (blokSutun+c)*HUCRE+1, (blokSatir+s)*HUCRE+1, HUCRE-2, HUCRE-2, ana);
                    Color n = kamuflaj[aktifTip][(s+c+1)%3];
                    Rect(SOL_PANEL + (blokSutun+c)*HUCRE+6,  (blokSatir+s)*HUCRE+6,  8, 8, n);
                    Rect(SOL_PANEL + (blokSutun+c)*HUCRE+18, (blokSatir+s)*HUCRE+14, 6, 6, n);
                    Rect(SOL_PANEL + (blokSutun+c)*HUCRE+10, (blokSatir+s)*HUCRE+20, 7, 5, n);
                }

        // Izgara
        for (int s = 0; s <= SATIR; s++)
            Rect(SOL_PANEL, s*HUCRE, SUTUN*HUCRE, 1, new Color(40,55,35));
        for (int c = 0; c <= SUTUN; c++)
            Rect(SOL_PANEL + c*HUCRE, 0, 1, SATIR*HUCRE, new Color(40,55,35));

        // Sag panel - kayan yazi
        int sagX = SOL_PANEL + SUTUN*HUCRE;
        Rect(sagX, 0, SAG_PANEL, SATIR*HUCRE, new Color(15,25,10));
        Rect(sagX+5, 5, SAG_PANEL-10, 2, new Color(180,0,0));
        Rect(sagX+5, SATIR*HUCRE-10, SAG_PANEL-10, 2, new Color(180,0,0));

        for (int i = 0; i < hitabe.Length; i++)
        {
            int satırY = (int)(kaymaY + i * 22);
            if (satırY < -22 || satırY > SATIR * HUCRE) continue;
            Color hRenk = i == 0 ? new Color(220,180,50) : Color.LightGray;
            if (hitabe[i].StartsWith("-")) hRenk = new Color(180,0,0);
            Yazi(hitabe[i], sagX + 8, satırY, hRenk);
        }

        // Bravo
        if (bravoGoster)
        {
            Rect(SOL_PANEL+10, SATIR*HUCRE/2-25, SUTUN*HUCRE-20, 50, new Color(0,80,0,220));
            Yazi("Bravo, Asker!", SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2, Color.Yellow, true);
        }

        // Oyun bitti
        if (oyunBitti)
        {
            Rect(SOL_PANEL+5, SATIR*HUCRE/2-55, SUTUN*HUCRE-10, 110, new Color(100,0,0,235));
            Yazi("Buraya kadarmis, Asker!", SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2-30, Color.White,      true);
            Yazi("Puan: " + puan,           SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2,    Color.Yellow,     true);
            Yazi("[ R ] Tekrar Oyna",       SOL_PANEL + SUTUN*HUCRE/2, SATIR*HUCRE/2+30, Color.LightGreen, true);
        }

        _spriteBatch.End();
    }

    void Rect(int x, int y, int w, int h, Color c)
    {
        _spriteBatch.Draw(kutu, new Rectangle(x, y, w, h), c);
    }

    void Yazi(string metin, int x, int y, Color renk, bool ortala = false)
    {
        if (font == null) return;
        string temiz = Temizle(metin);
        Vector2 boyut = font.MeasureString(temiz);
        Vector2 pos = ortala
            ? new Vector2(x - boyut.X/2, y - boyut.Y/2)
            : new Vector2(x, y);
        _spriteBatch.DrawString(font, temiz, pos, renk);
    }
}
