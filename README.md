# Forest-Parser
does parsing of the "https://www.lesegais.ru/open-area/deal" page

Работы программы:
  Время первого прохода - около 26 минут:
    ~1 минута на сбор данных сайта,
    почти всё остальное время занимает выполнение
    запросов SQL Server.
  Время последующих проходов должно колебаться в пределах
  5-26 минут, среднее - 16 минут.
  Интервал между проходами - 10 минут.
В качестве уникального ключа выбран номер сделки.
Повторяющиеся значения будут игнорироваться при вставке.
Строка подключения к DB находится в App.config.
Размер выборки(количество сделок на странице) - 9990 сделок, размер вставки в базу данных - 999 сделок.

  ![image](https://user-images.githubusercontent.com/103505023/180056220-2a72b01e-9d72-4f92-8a25-521abab626c7.png)
