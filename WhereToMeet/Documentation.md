# Documentation of Where To Meet? API #

Welcome to this documentation :raised_hands: !

:arrow_right: This documentation will explain how to use the API of Where to Meet?, function by function, and with emoji :blush: !

:arrow_right: The URL of the API (referred as *URL_API*) is: http://wheretomeet-api.azurewebsites.net/

:arrow_right: The parameters are given via query parameters (Request Parameters).


## Register ##
### Query :information_desk_person: ###
The Registration (Join) is performed using:

| *Route*  | `/api/register`|
|----------|----------------|
| *Method* | POST           |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/register"`.
The parameters are:

| *Parameter name*|  *Explanation*            |
|-----------------|---------------------------|
| username        | The username of the user. |
| password        | The password of the user. |
| email           | The email of the user.    |

### Return value :heart_eyes_cat: ###
##### If success :+1: : 
Returns code `200` (OK).
##### If failure :-1:  : 
Returns code `400` (Bad Request). In this case, the request is not valid.
Returns an error message.

## Log In ##
### Query :information_desk_person: ###
The Log In is performed using:

| *Route*  | `/api/login` |
|----------|--------------|
| *Method* | POST         |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/login"`.

The parameters are:

| *Parameter name*|  *Explanation*            |
|-----------------|---------------------------|
| username        | The username of the user. |
| password        | The password of the user. |

### Return value :heart_eyes_cat: ###
##### If success :+1: : 
Returns code `200` (OK).
The return value is a token, that should be passed through the Authorization HTTP header, with the Bearer scheme.

```json
{
	"appToken": "iTSBE1V4kNke1w0SmuY7EQkfE8lmuuBm28GoekgD2ZmtqTDQFV8K96gccfoYqmcTxT6rs0JDj5THq5oNXMARA8jRgEvwYN7D1F9"
}

```

##### If failure :-1: : 

Returns code `400` (Bad Request). In this case, the user is not found, or the request is not valid.
Returns an error message.

#### :milky_way: Usage of the token :smile: ####

The token should be passed through the *Authorization HTTP Header*, with the *Bearer* scheme.

| *Header Key*      | Authorization         |
|-------------------|-----------------------|
| *Header Value*    | `"Bearer " + appToken`|

*If the authentication fails* :thumbsdown:, the query will return code `401` (Unauthorized).

##### Example with loopj Async Http library:

```java
    client.addHeader("Authorization", "Bearer " + appToken);
```

The variable `client` is the instance of the AsyncHttpClient.

## Values (for testing querying values) ##

### Authentication
This route uses [the authentication process described here](#milky_way-usage-of-the-token-smile).

### Query :information_desk_person: ###
Get values using:

| *Route*  | `/api/values` |
|----------|---------------|
| *Method* | GET           |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/values"`.

There are no parameters given.

### Return value :heart_eyes_cat: ###
#### If success :+1: : ####

Returns code `200` (OK).
The return value is a list of string values.

```json
[
	"안녕하세요",
	"저는",
	"프랑스인",
	"입니다"
]

```

#### If failure :-1:  : ####

Undefined behaviour.


## Location (To inform the server on the user's location) ##

### Authentication
This route uses [the authentication process described here](#milky_way-usage-of-the-token-smile).

### Query :information_desk_person: ###
Update the Location using:

| *Route*  | `/api/location` |
|----------|-----------------|
| *Method* | PUT             |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/location"`.

The parameters are required:

| *Parameter name*|  *Explanation*                                            |
|-----------------|-----------------------------------------------------------|
| latitude        | Cartesian coordinate of the user on earth. == Y position. |
| longitude       | Cartesian coordinate of the user on earth. == X position. |

> NB: The parameters must be formatted in the UK notation, for example: 32.69862547

### Return value :heart_eyes_cat: ###
#### If success :+1: : ####

Returns code `200` (OK).
No return value.

#### If failure :-1:  : ####

Returns code `400` (Bad Request).




## Perfect Place ##

### Authentication
This route uses [the authentication process described here](#milky_way-usage-of-the-token-smile).

### Query :information_desk_person: ###
Get the Perfect Place using:

| *Route*  | `/api/perfectplace` |
|----------|---------------------|
| *Method* | GET                 |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/perfectplace"`.

The parameters are required:

| *Parameter name*|  *Explanation*                                                                |
|-----------------|-------------------------------------------------------------------------------|
| types           | Array of the place types. This array contains strings.                        |
| participants    | Array of the Friends Ids of the new group. This array contains integers.	  |

#### How to add array as parameters with Loopj Async Http

Use [this function of the library](https://loopj.com/android-async-http/doc/com/loopj/android/http/RequestParams.html#put-java.lang.String-java.lang.Object-).

Example:
```java
ArrayList<int> participants = new ArrayList<int>();
participants.add(123);
participants.add(13);
participants.add(91600);
RequestParams req = new RequestParams();

req.put("participants", participants);

```

### Return value :heart_eyes_cat: ###
#### If success :+1: : ####

Returns code `200` (OK) or `204` (No Content).
Returns the Perfect Place.

> :warning: :raised_hand: This request __can return code `204`__ if nothing was found. In this case, an error message should be displayed.

```json
{
  "name": "바오밥홍대본점",
  "latitude": 126.9226642,
  "longitude": 37.5492454,
  "tags": [
    "bar",
    "point_of_interest",
    "establishment"
  ]
}
```

#### If failure :-1:  : ####

Returns code `400` (Bad Request).

## Friends GET ##

### Authentication
This route uses [the authentication process described here](#milky_way-usage-of-the-token-smile).

### Query :information_desk_person: ###
Get the Friends using:

| *Route*  | `/api/friends`      |
|----------|---------------------|
| *Method* | GET                 |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/friends"`.

No parameters are required.

### Return value :heart_eyes_cat: ###
#### If success :+1: : ####

Returns code `200` (OK).

The application returns the friends list.
```json
[
  {
    "id": 26,
    "username": "RoHi",
    "email": "rohi@naver.com",
    "lastKnownY": 37.5522255,
    "lastKnownX": 126.9233862
  },
  {
    "id": 27,
    "username": "InSeo",
    "email": "inseo@naver.com",
    "lastKnownY": 38.3547149,
    "lastKnownX": 125.4809314
  },
  {
    "id": 28,
    "username": "JungHeum",
    "email": "jungheum@naver.com",
    "lastKnownY": 37.5156865,
    "lastKnownX": 126.9960697
  },
  {
    "id": 29,
    "username": "YongHan",
    "email": "yonghan@naver.com",
    "lastKnownY": 37.538427037,
    "lastKnownX": 126.9654440126
  }
]
```

#### If failure :-1:  : ####

Returns code `400` (Bad Request).



## Friends POST (Add a friend by username) ##

### Authentication
This route uses [the authentication process described here](#milky_way-usage-of-the-token-smile).

### Query :information_desk_person: ###
Add a new friend using:

| *Route*  | `/api/friends`      |
|----------|---------------------|
| *Method* | POST                |

To use it, just add it to the end of the *URL_API*, just like that: `URL_API + "/api/friends"`.

This request requires the following parameters:

| *Parameter name*|  *Explanation*                                        |
|-----------------|-------------------------------------------------------|
| username        | Username of the user to be added.				      |


### Return value :heart_eyes_cat: ###
#### If success :+1: : ####

Returns code `200` (OK).

The API returns the new friends list.

```json
[
  {
    "id": 26,
    "username": "RoHi",
    "email": "rohi@naver.com",
    "lastKnownY": 37.5522255,
    "lastKnownX": 126.9233862
  },
  {
    "id": 27,
    "username": "InSeo",
    "email": "inseo@naver.com",
    "lastKnownY": 38.3547149,
    "lastKnownX": 125.4809314
  }
]
```

#### If failure :-1:  : ####

Returns code `400` (Bad Request). With an error message from below:

| *Error messages*                                        |
|---------------------------------------------------------|
| `Username is empty`									  |
| `User is not found`									  |
| `Friend is already added.`							  |