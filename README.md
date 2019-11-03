# PleaseTakes 2
A much-improved implementation of the *PleaseTakes* software that I implemented for my old high school back in the summer of 2010. This second version employs many of the different techniques I learnt during the first two years of my undergradute studies in Computing Science at the University of Glasgow.

You can find the [original PleaseTakes implementation here](https://www.github.com/maxwelld90/pleasetakes/); it's a huge improvement, both in terms of the technologies used, and the engineering concepts that I employed.

## What is PleaseTakes 2?
As previously mentioned, this is the second release of a software project called PleaseTakes that I developed for my high school, [Mearns Castle High School](https://blogs.glowscotland.org.uk/er/MearnsCastle/). It automates the concept of a *please take*, where a teacher is told to go and cover the class for a teacher who for whatever reason cannot teach the class themselves.

The system allows administrators to select staff who are absent, identify what periods of the day they are absent (either the whole day, or a specific timetabled period), and select a member of staff who is free during that period/those periods to cover any scheduled classs. At the end of the process, administrators can print out a series of slips that are then distributed to the lucky teachers who have been selected to cover classes.

PleaseTakes was used in various guises from 2006 (when I was in third/fourth year at high school, or 15/16) to 2015, when the local education authority insisted that the functionality of the software should be taken over by an approved contractor. Fair enough. I probably missed a nice business opportunity here. But at the time, I valued my studies more than setting up a business.

## Technologies Used
This is a vast improvement over the existing implementation. As I was going through my undergraduate computing science degree, I now had knowledge of object-orientated programming concepts, and a much-improved understanding of relational databases. I discovered a pure love of engineering a system from a blank slate, and looking back at my code here, I'm quite proud of this! However, I didn't yet have a solid appreciation of contemporary web application frameworks (like Django), so I have implemented many of the underlying core concepts in C#... like a rudimentary model-view-template architecture pattern. Which is mental. But whatever. I had fun, and it worked well for me.

Using C#, you can tell immediately I was still tied into the Microsoft ecosystem at this point. The technologies used are listed below.

* Microsoft Internet Information Servcies (IIS) server, version 6.0
* Microsoft Visual Studio 2008, using C# version 3.0
* Microsoft SQL Server 2005 Express Edition

## Why Host this on GitHub?
I don't expect anybody to utilise this software; it probably doesn't work in this current state, and requires a fair amount of configuration to get working on a contemporary Windows Server instance. I put it up to demonstrate to the world that *I can code*, and that *I can provide a successful implementation to problems that require complex solutions*. A lot of planning went into this project, and this is something that I'm proud of but haven't shared with anyone until November 2019.

## Can I use the Code?
If you want to use this code, or simply talk to me about it, please contact me first. Do your due diligence. I'm not going to support or maintain it; it's here purely for achival purposes.

## The Database
Check out the database schema! There's a huge PNG file stored in the repository root that demonstrates all the different tables and relationships that I came up. It's a monster, and I was very proud of it at the time. There's way more tables than there needs to be because I had grander plans for this software other than PleaseTakes. These never came to fruition, however. A shame. Could have been fun and successful.

## Issues
There's plenty of things that I would do differently nowadays with the knowledge I now possess. One of the big issues for me that this software didn't really address was security; passwords were stored as plaintext, for example. I hadn't yet been taught the importance of password salting, something that I was taught in my third year. Or the importance of HTTPS; but that is deployment-specific. I could have made use of the .NET web frameworks a lot more, but then again, I wanted the challenge of working from a clean slate.

The software is not without its flaws; it's far from perfect, but it does demonstrate a significant improvement over my previous implementation. I'm always learning, and in a contemporary implementation, this would definitely be hosted on the cloud using purely open-source software. Probably in Docker instances that I could spin up and down at will. 🤓 Maybe I should reimplement it! There's plenty of things I'd like to do differently...

**Enjoy! And please remember: this is archived; this is not being actively developed. It's just here to show you that I can code. And I know for a fact that in 2019, I can code a lot better than I could code in 2010.**