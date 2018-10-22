# CustodyRequestFromPlatform

### Descripción General

Permite la creación de una Custodia. Dicha Custodia, enviará un SMS con la configuración estipulada en el producto, generando un código único y alias asociado, que poseerá una vigencia de 20 minutos.
<br>Cabe mencionar que este servicio será gatillado principalmente por servicio de alta de la plataforma.
<br>

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/custodyrequestfromplatform |
| Endpoint Producción | https://api.digevo.com/users/custodyrequestfromplatform     |
| Protocolo           | GET                                               |

<br>

## Request

### Parámetros (QueryString, tipo: https://www.tuurl.com/?param1=val1&param2=val2&paramN=valN)

| Atributo          | Tipo    | Descripción               | Obligatorío |
|-------------------|---------|---------------------------|:-----------:|
| idProduct | Integer | ID del producto enviado por la plaforma  | Sí          |
| ani       | String  | Número telefónico enviado por la plataforma      | Sí          |
| value     | String  | Valor del canal (ANI) enviado por la plataforma        | Sí          |


### Consumir API desde Javascript (jQuery)

```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "{ENV}/custodyrequestfromplatform",
  "method": "GET",
  "headers": {
    "content-type": "application/json",
    "x-api-key": "{YOUR_KEY}",
    "cache-control": "no-cache"
  },
  "processData": false
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Consumir API desde PHP (cURL)

```php
<?php

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "{ENV}/custodyrequestfromplatform",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "GET",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: {YOUR-KEY}"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Consumir API desde C# (RestSharp)

```c#
var client = new RestClient("{ENV}/custodyrequestfromplatform");
var request = new RestRequest(Method.GET);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "{YOUR-KEY}");
request.AddHeader("content-type", "application/json");
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
		"idCustody":1028,
		"alias":"gy1GH9",
		"code":"d65a00d4-2c8f-42db-af1e-2846a1b7913d"
	}
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "Producto no existe en el sistema"
    }
}
```

<br><br><br>




# CloseCustody

### Descripción General

Cierra la Custodia en función de su identificador.
<br>

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/closecustody |
| Endpoint Producción | https://api.digevo.com/users/closecustody     |
| Protocolo           | PUT                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorío |
|-------------------|---------|---------------------------|:-----------:|
| idCustody | Integer | ID de Custodia  | Sí          |


### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/closecustody",
  "method": "PUT",
  "headers": {
    "content-type": "application/json",
    "x-api-key": "[YOUR-KEY]",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\"idCustody\":[YOUR-VALUE]}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php

// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "[ENV]/closecustody",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "PUT",
  CURLOPT_POSTFIELDS => "{\"idCustody\":[YOUR-VALUE]}",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE] por valores correspondientes

var client = new RestClient("[ENV]/closecustody");
var request = new RestRequest(Method.PUT);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddHeader("content-type", "application/json");
request.AddParameter("application/json", "{\"idCustody\":[YOUR-VALUE]}", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "updated": "2017-10-04T10:24:07.5235504-03:00"
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 400,
        "message": "No se ha recibido ningún parámetro"
    }
}
```

<br><br><br>


# Register()

### Descripción General

Registro sobre cualquier tipo de canal definido dentro del módulo. Conlleva el concepto de credencial implícito.


### Descripción Técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/register |
| Endpoint Producción | https://api.digevo.com/users/register     |
| Protocolo           | POST                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idProduct | Integer | ID del producto sobre el que se está registrando  | Sí          |
| idChannel | Integer | ID del canal por el cual se está registrando  | Sí          |
| value | String | Valor que será almacenado en su identificador de usuario  | Sí          |
| code | String | Si el proceso de registro está asociado a custodia  | No          |
| codeType | String | Si el código va, debe enviarse el tipo (valores posibles: short o long), short corresponde al alias, long	al código completo  | No          |
| password | String | Si la credencial asociada tendrá o no contraseña  | No          |

### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/register",
  "method": "POST",
  "headers": {
    "x-api-key": "[YOUR-KEY]",
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\",\r\n\t\"code\":\"[YOUR-VALUE-4]\",\r\n\t\"codeType\": \"[YOUR-VALUE-5]\"\r\n}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_PORT => "58591",
  CURLOPT_URL => "[ENV]/register",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS => "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\",\r\n\t\"code\":\"[YOUR-VALUE-4]\",\r\n\t\"codeType\": \"[YOUR-VALUE-5]\"\r\n}",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

var client = new RestClient("[ENV]/closecustody");
var request = new RestRequest(Method.PUT);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddHeader("content-type", "application/json");
request.AddParameter("application/json", "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\",\r\n\t\"code\":\"[YOUR-VALUE-4]\",\r\n\t\"codeType\": \"[YOUR-VALUE-5]\"\r\n}", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "idClient": 44
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 409,
        "message": "El código proporcionado corresponde a una custodia expirada"
    }
}
```

<br><br><br>


# Login()

### Descripción General

Verifica si el usuario existe con las credenciales proporcionadas


### Descripción Técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/login |
| Endpoint Producción | https://api.digevo.com/users/login     |
| Protocolo           | POST                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idProduct | Integer | ID del producto sobre el que se está registrando  | Sí          |
| idChannel | Integer | ID del canal por el cual se está registrando  | Sí          |
| value | String | Valor que será almacenado en su identificador de usuario  | Sí          |
| password | String | Si la credencial asociada tendrá o no contraseña  | No          |

### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/register",
  "method": "POST",
  "headers": {
    "x-api-key": "[YOUR-KEY]",
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "[ENV]/register",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS => "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

var client = new RestClient("[ENV]/closecustody");
var request = new RestRequest(Method.POST);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddHeader("content-type", "application/json");
request.AddParameter("application/json", "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "idClient": 45
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "La identidad del usuario no existe en el sistema"
    }
}
```

<br><br><br>


# ResetPassword()

### Descripción General

Resetea contraseña de usuario si eventualmente está asociada a las credenciales enviadas


### Descripción Técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/resetpassword |
| Endpoint Producción | https://api.digevo.com/users/resetpassword     |
| Protocolo           | PUT                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idProduct | Integer | ID del producto sobre el que se está registrando  | Sí          |
| idChannel | Integer | ID del canal por el cual se está registrando  | Sí          |
| value | String | Valor que será almacenado en su identificador de usuario  | Sí          |


### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/register",
  "method": "PUT",
  "headers": {
    "x-api-key": "[YOUR-KEY]",
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "[ENV]/register",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "PUT",
  CURLOPT_POSTFIELDS => "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

var client = new RestClient("[ENV]/closecustody");
var request = new RestRequest(Method.PUT);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddHeader("content-type", "application/json");
request.AddParameter("application/json", "{\r\n\t\"idProduct\":[YOUR-VALUE-1],\r\n\t\"idChannel\":[YOUR-VALUE-2],\r\n\t\"value\":\"[YOUR-VALUE-3]\"\r\n}", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "newPassword": "B0kRLNP7miui"
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "La identidad del usuario no existe en el sistema"
    }
}
```

<br><br><br>






# UpdatePassword()

### Descripción General

Actualiza contraseña de usuario


### Descripción Técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/updatepassword |
| Endpoint Producción | https://api.digevo.com/users/updatepassword     |
| Protocolo           | PUT                                               |


## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idClient | Decimal | ID del usuario  | Sí          |
| idProduct | Integer | ID del producto sobre el que se está operando  | Sí          |
| idChannel | Integer | ID del canal sobre la solicitud de reseteo  | Sí          |
| oldPassword | String | Contraseña actual que desea actualizar  | Sí          |
| newPassword | String | Nueva contraseña  | Sí          |
| newPasswordRe | String | Reenvío de nueva contraseña para validación  | Sí          |


### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/updatepassword",
  "method": "PUT",
  "headers": {
    "x-api-key": "[YOUR-KEY]",
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\r\n\t\"idClient\":[YOUR-VALUE-1],\r\n\t\"idProduct\":[YOUR-VALUE-2],\r\n\t\"idChannel\":[YOUR-VALUE-3],\r\n\t\"oldPassword\":\"[YOUR-VALUE-4]\",\r\n\t\"newPassword\":\"[YOUR-VALUE-5]\",\r\n\t\"newPasswordRe\":\"[YOUR-VALUE-6]\"\r\n}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "[ENV]/updatepassword",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "PUT",
  CURLOPT_POSTFIELDS => "{\r\n\t\"idClient\":[YOUR-VALUE-1],\r\n\t\"idProduct\":[YOUR-VALUE-2],\r\n\t\"idChannel\":[YOUR-VALUE-3],\r\n\t\"oldPassword\":\"[YOUR-VALUE-4]\",\r\n\t\"newPassword\":\"[YOUR-VALUE-5]\",\r\n\t\"newPasswordRe\":\"[YOUR-VALUE-6]\"\r\n}",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

var client = new RestClient("[ENV]/updatepassword");
var request = new RestRequest(Method.PUT);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("content-type", "application/json");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddParameter("application/json", "{\r\n\t\"idClient\":[YOUR-VALUE-1],\r\n\t\"idProduct\":[YOUR-VALUE-2],\r\n\t\"idChannel\":[YOUR-VALUE-3],\r\n\t\"oldPassword\":\"[YOUR-VALUE-4]\",\r\n\t\"newPassword\":\"[YOUR-VALUE-5]\",\r\n\t\"newPasswordRe\":\"[YOUR-VALUE-6]\"\r\n}", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "updated": "2017-10-04T15:07:48.7529032-03:00"
    }
}
```

### Estructura respuesta de error (ejemplo)

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 400,
        "message": "La contraseña no coincide"
    }
}
```

<br><br><br>




# FillUserData()

### Descripción General

Completa o inicia información del usuario en relación a los atributos disponibles (como meta-key),
permitiendo multi-envío de valores para realizar el proceso en una única petición.


### Descripción Técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/filluserdata |
| Endpoint Producción | https://api.digevo.com/users/filluserdata     |
| Protocolo           | POST                                               |


## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idClient | Decimal | ID del usuario  | Sí          |
| tags | String[] | Arreglo de TAGs que identifican los parámetros relacionados  | Sí          |
| values | String[] | Valores relacionados a los tags e idClient (meta-key).  | Sí          |


### Ejemplo Javascript (jQuery)

```javascript
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "[ENV]/filluserdata",
  "method": "POST",
  "headers": {
    "x-api-key": "[YOUR-KEY]",
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\n    \"idClient\": 45,\n    \"tags\": [\n        \"APE_PATERNO\",\n        \"APE_MATERNO\"\n    ],\n    \"values\": [\n        \"Barraza\",\n        \"Non Schweinsteiger\"\n    ]\n}\n"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo PHP (cURL)

```php
<?php
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "[ENV]/filluserdata",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "POST",
  CURLOPT_POSTFIELDS => "{\n    \"idClient\": 45,\n    \"tags\": [\n        \"APE_PATERNO\",\n        \"APE_MATERNO\"\n    ],\n    \"values\": [\n        \"Barraza\",\n        \"Non Schweinsteiger\"\n    ]\n}\n",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: [YOUR-KEY]"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo C# (RestSharp)

```c#
// Reemplazar [ENV], [YOUR-KEY], [YOUR-VALUE-N] por valores correspondientes

var client = new RestClient("[ENV]/filluserdata");
var request = new RestRequest(Method.POST);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("content-type", "application/json");
request.AddHeader("x-api-key", "[YOUR-KEY]");
request.AddParameter("application/json", "{\n    \"idClient\": 45,\n    \"tags\": [\n        \"APE_PATERNO\",\n        \"APE_MATERNO\"\n    ],\n    \"values\": [\n        \"Barraza\",\n        \"Non Schweinsteiger\"\n    ]\n}\n", ParameterType.RequestBody);
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "numberOfItemsInserted": 2
    }
}
```

### Estructura respuesta de error (ejemplo)

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "No existe el usuario en el sistema"
    }
}
```

<br><br><br>




# GetChannelList

### Descripción General

Listado de canales disponibles en el sistema.
<br>

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/getchannellist |
| Endpoint Producción | https://api.digevo.com/users/getchannellist     |
| Protocolo           | GET                                               |

<br>

## Request

### Ejemplo desde Javascript (jQuery)

```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "{ENV}/getchannellist",
  "method": "GET",
  "headers": {
    "content-type": "application/json",
    "x-api-key": "{YOUR_KEY}",
    "cache-control": "no-cache"
  },
  "processData": false
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo desde PHP (cURL)

```php
<?php

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "{ENV}/getchannellist",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "GET",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: {YOUR-KEY}"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo desde C# (RestSharp)

```c#
var client = new RestClient("{ENV}/getchannellist");
var request = new RestRequest(Method.GET);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "{YOUR-KEY}");
request.AddHeader("content-type", "application/json");
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": [
        {
            "idChannel": 1,
            "name": "Ani",
            "description": "Número de teléfono móvil"
        },
        {
            "idChannel": 2,
            "name": "E-mail",
            "description": "Correo electrónico (WP por ejemplo)"
        },
        {
            "idChannel": 3,
            "name": "Facebook",
            "description": "Facebook"
        },
        {
            "idChannel": 4,
            "name": "Operator",
            "description": "Related to RegisterOperator"
        }
    ]
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "No existen canales disponibles"
    }
}
```

<br><br><br>



# GetParamList()

### Descripción General

Obtiene listado de parámetros disponibles en el módulo (meta-key para usuario)

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/getparamlist |
| Endpoint Producción | https://api.digevo.com/users/getparamlist     |
| Protocolo           | GET                                               |

<br>

## Request

### Ejemplo desde Javascript (jQuery)

```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "{ENV}/getparamlist",
  "method": "GET",
  "headers": {
    "content-type": "application/json",
    "x-api-key": "{YOUR_KEY}",
    "cache-control": "no-cache"
  },
  "processData": false
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Ejemplo desde PHP (cURL)

```php
<?php

$curl = curl_init();

curl_setopt_array($curl, array(
  CURLOPT_URL => "{ENV}/getparamlist",
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => "",
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 30,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => "GET",
  CURLOPT_HTTPHEADER => array(
    "cache-control: no-cache",
    "content-type: application/json",
    "x-api-key: {YOUR-KEY}"
  ),
));

$response = curl_exec($curl);
$err = curl_error($curl);

curl_close($curl);

if ($err) {
  echo "cURL Error #:" . $err;
} else {
  echo $response;
}
```

### Ejemplo desde C# (RestSharp)

```c#
var client = new RestClient("{ENV}/getparamlist");
var request = new RestRequest(Method.GET);
request.AddHeader("cache-control", "no-cache");
request.AddHeader("x-api-key", "{YOUR-KEY}");
request.AddHeader("content-type", "application/json");
IRestResponse response = client.Execute(request);
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": [
        {
            "idParam": 1,
            "name": "Nombre",
            "description": "Nombre usuario",
            "tag": "NOMBRE",
            "creationDate": "2017-09-26T00:00:00",
            "modificationDate": null
        },
        {
            "idParam": 3,
            "name": "ApePaterno",
            "description": "Apellido paterno",
            "tag": "APE_PATERNO",
            "creationDate": "2017-09-26T00:00:00",
            "modificationDate": null
        },
        {
            "idParam": 4,
            "name": "ApeMaterno",
            "description": "Apellido materno usuario",
            "tag": "APE_MATERNO",
            "creationDate": "2017-09-26T00:00:00",
            "modificationDate": null
        }
    ]
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "No existen parámetros disponibles en el sistema"
    }
}
```

<br><br><br>

# Unregister()

### Descripción General

Deshabilita acceso de usuario (específicamente, modifica estado de credencial)

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/users/unregister |
| Endpoint Producción | https://api.digevo.com/users/unregister     |
| Protocolo           | PUT                                               |

<br>

## Request

### Ejemplo de estructura

```json
{
	"idProduct": 1,
	"idChannel": 2,
	"value": "xxx@yyy.com",
	"password": "abc123"		// optional, only if user was registered with it
}
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "data": {
        "updated": "2017-10-04T15:07:48.7529032-03:00"
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "users",
    "error": {
        "code": 204,
        "message": "No existen parámetros disponibles en el sistema"
    }
}
```