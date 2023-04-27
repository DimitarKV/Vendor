## Vendor

![Logo](https://raw.githubusercontent.com/BatMitio/Vendor/main/resources/Vendor_logo.PNG)

## Description

Vendor is a strong business oriented architecture.  
for managing and operating large quantities of  
different types of vending machines. The modern  
technologies used allow for a huge flexibility in  
the hardware deployed on site. The  
application is a standalone network with a centralized  
architecture and allows for different layers of access  
to the resources it maintains.

## Prerequisite

In order for the application to run properly you should have a locally running MsSQL server with a user login profile available. There should be user-secrets setup for each individual service (.Api or portal.Blazor project)

```plaintext
    dotnet user-secrets init
    dotnet user-secrets set "Key" "Value"
```

### For all services except Blazor portal

```plaintext
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:DbUser" "username"
    dotnet user-secrets set "ConnectionStrings:DbPassword" "password"
```

### For Users service

```plaintext
    dotnet user-secrets set "SendGridKey" "sendgridkey"
    dotnet user-secrets set "OutboundAddress" "outboundaddress"
```

### For Products service

```plaintext
    dotnet user-secrets set "Cloudinary:CloudName" "cloudname"
    dotnet user-secrets set "Cloudinary:APISecret" "apisecret"
    dotnet user-secrets set "Cloudinary:APIKey" "apikey"
```

## When all settings are done

When you have finished configuring the application environment you just need to run the four services (Users, Machines, Products, Portal.Blazor - all https)

## Support

For support,  
email dimitar.k.vasilev.04@gmail.com.