
export function setTheme(themeName) {    
    //add a new css link
    let newLink = document.createElement("link");
    newLink.setAttribute("id", "theme");
    newLink.setAttribute("rel", "stylesheet");
    newLink.setAttribute("type", "text/css");
    newLink.setAttribute("href", `assets/css/${themeName}.css`);
    //remove the theme from the head tag
    let head = document.getElementsByTagName("head")[0];
    head.querySelector("#theme").remove();
    //adding newLink in the head
    head.appendChild(newLink);
}

