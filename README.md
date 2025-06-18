# 🎮 Jejak Rempah - Game Edukasi Platformer 3D Bertema Rempah Nusantara

**Jejak Rempah** adalah game casual/arcade 3D yang mengajak pemain menjelajah kepulauan Nusantara untuk mengumpulkan berbagai rempah-rempah khas Indonesia. Game ini dikembangkan sebagai adaptasi dan modifikasi dari template platformer 3D Godot dengan C#, yang dikembangkan lebih lanjut untuk memperkenalkan kekayaan budaya dan rempah Indonesia secara menyenangkan dan interaktif.

> 🧭 Catatan: Game ini merupakan hasil pengembangan dari *template/tutorial platformer 3D* berbasis Godot + C#, yang dimodifikasi dan diperluas untuk menjadi **Jejak Rempah**.

---

## 📌 Ringkasan Game

* **Judul**: Jejak Rempah
* **Genre**: Casual / Arcade / Platformer 3D
* **Platform**: PC / Android *(pengembangan berjalan)*
* **Tools**: [Godot Engine](https://godotengine.org/) + C#
* **Developer**: Fitri Salwa (231524009)
* **Target Usia**: 7–18 tahun dan keluarga pecinta game edukatif ringan

---

## 🧠 Analisis Kebutuhan

### 🎯 Target Pengguna

Game *Jejak Rempah* ditujukan untuk:

* Anak-anak dan remaja usia 7–18 tahun yang menyukai game edukatif
* Keluarga yang ingin mengenalkan budaya lokal melalui media digital
* Sekolah dan lembaga edukasi berbasis interaktif
* Pecinta budaya Nusantara dan casual gamer yang menyukai platformer ringan

### 👥 User Persona

| Nama    | Usia | Profil                             | Motivasi                             | Tantangan                                      |
| ------- | ---- | ---------------------------------- | ------------------------------------ | ---------------------------------------------- |
| Aulia   | 10   | Siswi SD kelas 4                   | Belajar sambil bermain               | Butuh kontrol dan instruksi yang mudah         |
| Raka    | 15   | Siswa SMP pecinta sejarah & budaya | Suka eksplorasi lokal lewat game     | Ingin game menantang dan tidak monoton         |
| Bu Dini | 38   | Guru SD dan ibu dua anak           | Mengenalkan budaya Nusantara ke anak | Membutuhkan game edukatif yang aman dan ringan |

### 💻 Kebutuhan Perangkat Lunak & Keras

| Kategori      | Spesifikasi Minimum                      |
| ------------- | ---------------------------------------- |
| **OS**        | Windows 7+, Android 7+                   |
| **Processor** | Dual Core 1.5 GHz                        |
| **RAM**       | 2 GB                                     |
| **GPU**       | Intel HD Graphics / Mali GPU (Android)   |
| **Storage**   | 200 MB (terkompres)                      |
| **Software**  | Godot Engine 4.3 + C#, Blender           |

### 🧪 Metode Pengumpulan Data

* **Referensi Studi Game Sejenis**: Subway Surfers, Temple Run, Super Mario Run
* **Studi Budaya Lokal**: Ensiklopedia Rempah, Peta Kuliner Nusantara, Rumah Adat
* **Observasi Tidak Langsung**: Analisis tutorial Godot & dokumentasi komunitas


---

## 🕹️ Konsep & Fitur Utama

### 🌍 Petualangan di 4 Pulau Nusantara

Setiap level mewakili daerah Indonesia dengan rempah khas, rintangan unik, dan musik tradisional lokal:

| Level | Pulau    | Rempah           | Bonus | Lingkungan Khas             | Musik Daerah |
| ----- | -------- | ---------------- | ----- | --------------------------- | ------------ |
| 1     | Jawa     | Jahe             | 2×    | Sawah terasering, Joglo     | Gamelan Jawa |
| 2     | Sumatera | Lada             | 3×    | Hutan tropis, Rumah Gadang  | Musik Melayu |
| 3     | Sulawesi | Cengkeh          | 5×    | Pegunungan, Tongkonan       | Musik Toraja |
| 4     | Maluku   | Pala, Kayu Manis | 7–10× | Vulkanik, Rumah Adat Maluku | Musik Maluku |

### 🎮 Gameplay Loop

* **Berlari & Melompat**: Hindari rintangan seperti lubang, batu, lava
* **Mengumpulkan**: Koin dan rempah sebagai item bonus
* **Unlock Level**: Naik level dan tingkatkan tantangan
* **Endless Mode**: Terbuka setelah menyelesaikan semua level utama

### ⚙️ Mekanik Inti

* Kontrol sederhana (`WASD` untuk gerak, `Space` untuk lompat)
* Sistem skor dan koin untuk membuka konten
* Power-up: kecepatan, ketahanan, magnet rempah
* Musik tradisional disesuaikan dengan daerah

---

## 📦 Asset Game

### 🧂 Rempah-rempah (3D Model)

* Jahe, Lada, Cengkeh, Pala, Kayu Manis (model low-poly)

### ⛔ Rintangan

* Lubang, batu, pohon tumbang, lava aktif, badai tropis

### 🏝️ Lingkungan

* Terasering, hutan tropis, pegunungan, vulkanik kepulauan

### 🎵 Musik Latar

* Musik tradisional `.mp3` dan `.ogg` dari setiap daerah (royalty-free & remix lokal)

---

## 🎯 Target Pengguna

* Anak dan remaja usia 7–18 tahun
* Keluarga yang menyukai game ringan edukatif
* Masyarakat yang ingin belajar tentang budaya Nusantara
* Sekolah atau lembaga edukasi interaktif berbasis digital

---

## 🔁 Progression System

| Tahap             | Level   | Fitur Baru                                     |
| ----------------- | ------- | ---------------------------------------------- |
| Penjelajah Pemula | Level 1 | Tutorial, koin & rempah dasar                  |
| Pengumpul Rempah  | Level 2 | Rintangan menantang, sistem combo              |
| Petualang Berani  | Level 3 | Environmental hazard, time challenge           |
| Master Nusantara  | Level 4 | Rintangan ekstrem, rempah langka, endless mode |

---

## 📚 Referensi Game Serupa

* [Subway Surfers](https://play.google.com/store/apps/details?id=com.kiloo.subwaysurf)
* [Temple Run](https://play.google.com/store/apps/details?id=com.imangi.templerun)
* [Super Mario Run](https://play.google.com/store/apps/details?id=com.nintendo.zara)

---

## 🎓 Tutorial Platformer 3D yang Menjadi Basis

Game ini dikembangkan dengan referensi dari tutorial:
🎥 [Godot Platformer 3D with C# – YouTube Playlist](https://youtube.com/playlist?list=PLeCKjxofwyfje6fOCDomz_vpvxT1mU-Ak&si=SJHPmxcSBK3FXRmE) *(oleh Chevifier)*

---

## 🚧 Status Pengembangan

* ✅ Level 1 & 2 selesai (jahe & lada)
* 🚧 Level 3 & 4 dalam pengembangan (cengkeh & pala)
* 🎨 Asset 3D sedang dalam proses tekstur dan polesan UI

---

## 🙌 Kontribusi & Kontak

Jika kamu tertarik untuk berkontribusi, memberikan saran, atau ingin menggunakan game ini sebagai media edukasi:

📧 Email: `ftrslwa8@gmail.com`
👩‍🎓 Dibuat sebagai tugas besar mata kuliah Komputer Grafik 

---

> 🎯 *Game Jejak Rempah bukan hanya menghibur, tetapi juga mengedukasi—mengangkat kekayaan rempah dan budaya Nusantara ke dunia digital yang interaktif dan menyenangkan.*

---
