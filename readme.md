# Markdown to HTML editor and Static Site Generator

## Version 1.0

This is mainly a WPF application that allows to work on Markdown files, convert them and view them under HTML format in a WPF window, and generate static HTML files ready to be uploaded to a Website.

The Markdown files are expected in a local folder `inputMarkdownFiles` defined in appsettings.json. Two types of HTML files are created:
- files to view in the WPF application
- files to upload to a web server through FTP

The main difference between these 2 types lies in the HTML template used and the included assets (CSS, JS). The content is the same, and obtained from the Markdown files

Features:
- Conversion of Markdown files to HTML using the Markdig library. HTML templates can be defined in order to encapsulate the markdown content
- Definition of a HTML template with a sidebar including a Table of Contents with links to each h1 and h2 tag
- FTP upload with the credentials defined in the appsettings.json file
- In-app edition of the original markdown files
- In-app edition of the CSS used for the HTML rendering in the WPF app

