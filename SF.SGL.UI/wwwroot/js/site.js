export function setTheme(themeName) {    
    
    let newLink = document.createElement("link");
    newLink.setAttribute("id", "theme");
    newLink.setAttribute("rel", "stylesheet");
    newLink.setAttribute("type", "text/css");
    newLink.setAttribute("href", `css/${themeName}.css`);
    
    let head = document.getElementsByTagName("head")[0];
    head.querySelector("#theme").remove();
    
    head.appendChild(newLink);
}

