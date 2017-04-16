# Documentation of Where To Meet? API #

Welcome to this documentation.
This documentation will explain how to use the API of Where to Meet?, function by function
The URL of the API (referred as *URL_API*) is: [to be created]
The parameters are given via query parameters (Request Parameters).

## Register ##
### Query ###
The Registration is performed using:

| *Route*  | /api/register|
|----------|--------------|
| *Method* | POST         |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/register"`.
The parameters are:

| *Parameter name*|  *Explanation*            |
|-----------------|---------------------------|
| username        | The username of the user. |
| password        | The password of the user. |
| email           | The email of the user.    |

### Return value ###
*If success:*
Returns code `200` (OK).
*If failure*
Returns code `400` (Bad Request). In this case, the request is not valid.
Returns an error message.

## Log In ##
### Query ###
The Log In is performed using:

| *Route*  | /api/login |
|----------|------------|
| *Method* | GET        |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/login"`.
The parameters are:

| *Parameter name*|  *Explanation*            |
|-----------------|---------------------------|
| username        | The username of the user. |
| password        | The password of the user. |

### Return value ###
*If success:*
Returns code `200` (OK).
The return value is a token, that should be passed through the Authorization HTTP header, with the Bearer scheme.

```json
{
	"appToken": "iTSBE1V4kNke1w0SmuY7EQkfE8lmuuBm28GoekgD2ZmtqTDQFV8K96gccfoYqmcTxT6rs0JDj5THq5oNXMARA8jRgEvwYN7D1F9"
}
*If failure*

Returns code `400` (Bad Request). In this case, the user is not found, or the request is not valid.
Returns an error message.


```
#### Usage of the token (with Loopj Async Http): ####

```java
    client.addHeader("Authorization", "Bearer " + appToken);
```
`client` being the instance of the HttpClient.

