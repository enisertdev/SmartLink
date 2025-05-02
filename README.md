
## 🏗️ Mimarî Yapı

Proje, **Onion Architecture** yapısına sahiptir. Bu yapı sayesinde katmanlar gevşek bağlıdır, test edilebilirlik ve sürdürülebilirlik sağlanır.

```
SmartLink/
├── SmartLink.Domain         # Temel domain modelleri ve arayüzler
├── SmartLink.Application    # Uygulama servisleri, DTO’lar, business logic
├── SmartLink.Infrastructure # EF Core, dış servisler, veri erişimi
├── SmartLink.API            # Web API katmanı
```

---

## 🛠️ Kullanılan Teknolojiler

- ✅ **.NET 8**
- 🧅 **Onion Architecture**
- 🧠 **Google Gemini Pro API** – Yapay zekâ destekli metin özetleme
- 🗃️ **Entity Framework Core** + **MSSQL**
- 🌐 **ASP.NET Core Web API** – RESTful servisler

---


