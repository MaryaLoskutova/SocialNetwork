# SocialNetwork

WebApi сервис для соцсети, который позволяет сделать следующее: 
* Зарегистрировать нового клиента. Клиент при регистрации указывает только имя. Имя должно состоять из букв и пробелов и быть не длиннее 64 символов 
* Подписать одного клиента на другого
* Выбрать топ наиболее популярных клиентов. Вызывающий может указать требуемое количество записей


### Методы

#### 1. Регистрация пользователя

`
POST /users/register
`

RequestBody:

```
{
  "name": "string"
}
```

Response: 
```
(Guid) userId
```

*PS:* 
- Предполагается расширяемость метода, поэтому данные об имени пользователя помещены в Body. Это позволит в дальнейшем добавить и другие данные о пользователе.
- Сервис внутренний, поэтому для дальнейшей работы с пользователем неоходим идентификатор пользователя.



#### 2. Подписка на пользователя

`
POST /users/{subscriberId}/subscribe
`

Parameters:

```
(Guid) userId 
```

Response: 

```
(Guid) subscriptionId
```

*PS:* 
- Данный метод является одним из действий пользователя. Поэтому id пользователя, отправившего заявку на подписку прописывается в url
- Пользователь не может подписаться дважды на одного и того же пользователя или на самого себя. Для того, чтобы можно было явно описать валидация и причиу ошибки, сначала пользователи запрашиваются из БД. В дальнейшем можно добавить кэш для хранения пользователей, на которых уже подписывались, чтобы уменьшить количество запросов в БД.
- Если есть несколько пользователей с одинаковым количеством подписчкой, то они сортируются по дате последнего обновления.



#### 3. Получение топ популярных пользователей

`
GET /users/top
`

Parameters:

```
(int) count 
```

Response: 

```
[
  {
    "name": "string",
    "subscribersCount": 0
  }
]
```

*PS:* 
- Поскольку на данный момент нет знаний о том, что на основе возвращаемых данных потребуются действия с каким то из пользователе и нужен будет userid, позвращаются только имя пользователя и количество подписчиков
- В ответ пеередается бизнес модель без конвертации в возвращаемую внешнюю модель, поскольку на данный момент не трубется приведение ответа к какому-то иному виду. При расширении требований может понадобится конвертация dto в новую модель
- Добавлен InMemoryCash на хранение всех пользователей с подписчиками. Предполагается, что пока система небольшая, данного способа кэширования должно хватить. Однако при увеличении количества пользвоателей, можно будет пересчитывать количество подписчиков через отдельный рег. процесс с сохранением пересчитанных значений в отдельную таблицу.  Либо через брокер сообщений.



### Swagger

Для удобства добавлен swagger с описанием методов, статусов и входных/выходных данных

/swagger/index.html 
