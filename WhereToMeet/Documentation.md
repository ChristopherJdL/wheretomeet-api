# Documentation of Where To Meet? #

Welcome to this documentation.
This documentation will explain how to use the API of Where to Meet?, function by function
The URL of the API (referred as *URL_API*) is: [to be created]
The parameters are transmitted via query parameters (Request Parameters).

## Log In ##
### Query ###
The Log In is performed using the route:
`/api/login`
To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/login"`.
the parameters are:

| Parameter name | Explanation               |
|----------------|---------------------------|
| username       | The username of the user. |
| password       | The password of the user. |

### Return value ###

```json
{
	"appToken": "iTSBE1V4kNke1w0SmuY7EQkfE8lmuuBm28GoekgD2ZmtqTDQFV8K96gccfoYqmcTxT6rs0JDj5THq5oNXMARA8jRgEvwYN7D1F9"
}

```