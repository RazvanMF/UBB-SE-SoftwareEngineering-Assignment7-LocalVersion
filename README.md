# CELEBRATION OF CAPITALISM - ASSIGNMENT 7 LOCAL REPOSITORY
Repository for the frontend transition from WPF Desktop to ASP.NET Core MVC w/ Blazor and Razor Pages.

Main project (frontend transition) is **Celebration of Capitalism - The Finale**, server and legacy projects from assignment 6 are also included.

I prepared an example for the login part, including created layout, model, view and controller in Razor, including routing part to either the Privacy Policy Page (a.k.a our error page for now) and the main page, sessions are included as well (i.e. user's ID is stored at login, is refresh-persistent and can be accessed via HttpContext programatically).

### Division of Labor - Frontend Transition (implies design, layout and functionality)
* Florin: Main window, the one with all the products.
* Rares: Product page, the one that shows the product's details if one is pressed from the main window.
* Dan: Reviews page, the one that shows all the reviews of a specific product.
* Dragos: Favorite Products page, the one that shows all favorite products a user has. Accesible from the header navigation bar in the user dashboard.
* Nico: Register page, self explanatory. Located in the header navigation bar when there aren't users logged in.
* Diana: General layout (header, footer, routing to logout, favorites page @ user main page). Routing examples are available in the main layout under the Shared Folder within Views.
* Razvan: Whatever's left in the login module.

#### Old pictures of NamespaceGPT's design can be accessed in the "readme-resources folder"

**Regarding design, carte blanche, do whatever's easier or more convenient for you.** The project comes with CSS Bootstrap included, but standalone CSS files also work.

### Resources
Considering our lack of shared knowledge regarding Blazor/Razor, I've gathered some resources I used to make the example part.

[Microsoft ASP.NET MVC Documentation](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio)

[Bootstrap Layout Generator](https://www.layoutit.com/build)

[Razor Syntax](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-8.0) - in case using Razor is more convenient than plain HTML

The project also comes included with JQuery, so the old labs @ Web Programming (Javascript, JQuery) can also be useful.

## It would be nice if all of these are done by Saturday night.
Something important to mention is that I am not the supreme merge authority for this one, and this part should be passed on to one of the team leads @ 923/2.
***
# *that's it, love you, don't forget who we are, and all that.* ❤️