# Приложение для учета личных финансов

## Краткое описание идеи проекта
Разработка приложения, позволяющего вести учет личных финансов. После регистрации пользователь сможет создать несколько проектов для более удобного отслеживания расходов и доходов - семейный бюджет, личный бюджет, затраты на ремонт и так далее. Есть 3 типа пользователей - free, silver и gold, и от статуса пользователя зависит количество проектов, в которых он сможет участвововать. В проекте пользователь сможет добавлять записи об операциях, а также приглашать других пользователей для совместного учета общих финансов.

## Краткое описание предметной области
У каждого пользователя есть список проектов, в которых он может отслеживать финансы совместно с другими людьми. Если пользователь создает проект сам, он становится админом и может приглашать и удалять пользователей в своем проекте. Если пользователь принял приглашение в проект, он становится участником. И админ, и участник могут просматривать статистику расходов и доходов, а также добавлять записи с комментарием о новых операциях. Пользователь может просматривать записи отдельно по категориям. 
Есть ограничение - пользователь с уровнем free может участвовать не более чем в 3 проектах и создавать не более 1, с уровнем silver - участвовать в не более 6 и создавать не более 3, с уровнем gold пользователь может участвовать в безлимитном количестве проектов.

## Краткий анализ аналогичных решений

|  | Возможность разделить бюджет на несколько сфер | Возможность вести учет совместно с другими пользователями | Возможность самому задать период времени для вывода стастистики об операциях | Возможность комментировать операции|
|---|---|---|---|---|
|Money Lover |  + |  + |  - | + |
|CoinKeeper  |  - |  + |  - | + |
|Monefy      |  - |  + |  + | + |

## Краткое обоснование целесообразности и актуальности проекта
Актуально для людей, которые хотят анализировать количество расходов и доходов по категориям, отслеживать финансовые дыры в своем бюджете. Подойдет людям, у которых несколько сфер расходов - семья, работа, путешествие и так далее. Удобно для пользователей, которым важна возможность разделения бюджета с другими людьми - членами семьи, коллегами.

## Краткое описание акторов
Акторы:
- Гость - незарегистрированный пользователь;
- Авторизованный пользователь - прошедший авторизацию или регистрацию пользователь на стадии выбора проекта;
- Участник проекта - авторизованный пользователь относительно проекта, в который он принял приглашение;
- Админ проекта - авторизованный пользователь относительно проекта, который он создал.

## Use-Case - диаграмма

![Use-Case - диаграмма](docs/img/use_case.drawio.png)

#### 1 сложный use-case:
Авторизация

#### 2 сложный use-case:
Добавление пользователя в проект по логину. 2 проверки - наличие пользователя с таким логином и ограничение количества проектов в соответствии с уровнем пользователя.

## ER-диаграмма сущностей

![ER-диаграмма сущностей](docs/img/er_diagram.drawio.png)

## Пользовательские сценарии
Гость:
1. Зарегистрироваться
2. Авторизоваться

Авторизованный пользователь:
1. Выбрать проект
2. Создать свой проект
3. Принять приглашение в проект

Участник проекта:
1. Посмотреть список расходов и доходов за период времени
2. Добавить новую запись об операции
3. Выйти из проекта
4. Вернуться к выбору проектов

Админ проекта:
1. Посмотреть список расходов и доходов за период времени
2. Добавить новую запись об операции
3. Добавить нового участника в проект
4. Удалить участника из проекта
5. Удалить проект
6. Вернуться к выбору проектов

## Формализация ключевых бизнес-процессов

![BPMN диаграмма](docs/img/bpmn_diagram.svg)

## Технологический стек
Тип приложения: Web SPA

Backend:
- ASP.NET Core
- C#

Frontend:
- React

Database:
- PostgreSQL


## Диаграмма компонентов
 
![Диаграмма компонентов](docs/img/components_diag.drawio.png)

## UML диаграммы классов для компонента доступа к данным и компонента с бизнес-логикой
 
![uml диаграмма классов](docs/img/uml_diag.drawio.png)
