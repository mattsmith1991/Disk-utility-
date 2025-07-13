# # Disk Utility for Windows (Beta)

![Build](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)
![Version](https://img.shields.io/badge/version-v0.9--beta-orange)

A powerful Qt-based GUI disk management tool for Windows — blending the functionality of GParted, Rufus, Clonezilla, and Disk Drill in one unified interface.

---

## ✨ Features

- 💾 **Partition Management** – Create, delete, resize, align, and format partitions
- 🔥 **Bootable USB Tool** – Write Windows & Linux ISOs (BIOS + UEFI)
- 📂 **Multiboot USB Builder** – YUMI-style multiboot with persistence
- 📦 **Cloning** – Disk/partition cloning with split image support
- 🔍 **Recovery** – Deep scan engine with file previews (Disk Drill-style)
- 🧠 **Apple Filesystem** – Full APFS, HFS+, and CoreStorage support
- ⚙️ **Bootloader Management** – BCD editor, GRUB2 theming, EFI tools
- 🌐 **Multilingual UI** – English, Spanish, French (more soon)
- 🎨 **Dark/Light Themes** – Toggle in real-time
- 💿 **ISO Builder** – Export entire USB layout to a bootable ISO

---

## 🖥️ Requirements

- Windows 10/11 (64-bit)
- Qt 6.5+ (or use bundled installer)
- Admin privileges required for full features

---

## 🚀 Installation

### Option 1: Download Installer  
Grab the latest build from [Releases](https://github.com/yourusername/DiskUtility/releases)

### Option 2: Build from Source

```bash
git clone https://github.com/yourusername/DiskUtility.git
cd DiskUtility
mkdir build && cd build
cmake ..
cmake --build .