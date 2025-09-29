# ServerCoreBySeen

🎮 **ServerCoreBySeen** is a modular plugin pack for **CounterStrikeSharp** (CS2).  
It provides core server functionality with ranking, VIP perks, scoreboard customization, and more – designed to be **clean, configurable, and extendable**.

---

## ✨ Features

- **Ranking System**
  - XP & score tracking via SQLite
  - Configurable rewards for kills, headshots, bomb actions, and round results
  - XP loss on death / round loss (configurable)
  - Auto-creation of player entries on connect

- **VIP System**
  - Assign VIPs by SteamID
  - Configurable bonus XP & money
  - VIP scoreboard tags

- **Scoreboard Customization**
  - Tags for **TOP-1**, **VIP**, **ADMIN**, **DEV**
  - Toggleable in configuration

- **General Config**
  - Custom chat prefix
  - Admin menu command
  - Enable/disable each module

---

## ⚙️ Installation

1. Clone this repository into your CounterStrikeSharp plugin folder:
   ```sh
   git clone https://github.com/N0deNinja/ServerCoreBySeen.git
   ```
2. Build the project:
   ```sh
   dotnet build -c Release
   ```
3. Copy the compiled DLL from `bin/Release/net8.0/` to your CS2 `addons/counterstrikesharp/plugins` folder.
4. On first run, config files will be generated under:
   ```
   addons/counterstrikesharp/configs/plugins/ServerCoreBySeen/
   ```
5. Adjust configs to your needs.

---

## 🛠️ Configuration

### Example `MasterConfig.json`

```jsonc
{
  "General": {
    "ChatServerMessagesPrefix": "[Server Core By Seen]",
    "EnableVip": true,
    "EnableRanking": true,
    "EnableScoreboard": true,
    "AdminMenuCommand": "admin"
  },
  "Scoreboard": {
    "TopOneTag": "[TOP-1]",
    "VipTag": "[VIP]",
    "AdminTag": "[ADMIN]",
    "DevTag": "[DEV]"
  },
  "Vip": {
    "SteamIds": [],
    "ExtraExp": 100,
    "ExtraMoney": 500,
    "ExtraMoneyFromRound": 3,
    "ShowTag": true
  },
  "Ranking": {
    "ExpPerKill": 20,
    "ExpPerHeadshot": 10,
    "ExpPerBombDefuse": 50,
    "ExpPerBombPlant": 30,
    "ExpPerRoundWin": 40,
    "ExpLostPerRoundLoss": 10,
    "ExpLostPerDeath": 5,
    "MaxLevel": 0,
    "ShowTopOneTag": true
  }
}
```

---

## 📦 Tech Stack

- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)  
- C# / .NET 8  
- SQLite with Dapper ORM  

---

## 🚀 Roadmap

- [ ] Admin menu for live configuration  
- [ ] More VIP perks (weapons, effects, money, etc.)  
- [ ] Level-based rewards and abilities  

---

## 📜 License

This project is licensed under the **GPL v3 License**.  
See the [LICENSE](LICENSE) file for details.

---

👤 Author: **Seen (George Dvorak)**  
🔥 Contributions, PRs, and feature requests are welcome!
