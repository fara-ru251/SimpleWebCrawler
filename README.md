# SimpleWebCrawler
There for web scraping technique, I've used "Abot" library. It takes care of multithreading, HTTP requests and link parsing for ease, you've just deal with configuration.
For the crawl of page I've used "AngleSharp" library. This library follows the W3C specifications and gives you the same results as state of the art browsers. 
Node retrieval is straight forward by using powerful CSS query selectors. The CSS queries in AngleSharp are super fast and very simple to use.
For writing data to database I've used [Insight.Database](https://github.com/jonwagner/Insight.Database) lightweight ORM that is works pretty good with stored procedures.
