<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<ul id="menu">
    <li>
	    <a href="<%=Url.Action("index","home") %>">Home</a>
	</li>
    <li>
	    <a href="<%=Url.Action("index","newsletter") %>">Newsletter</a>
	</li>
	<li>
	    <a href="<%=Url.Action("index","noticias") %>">Notícias</a>
	</li>
    <li>
	    <a href="<%=Url.Action("index","contato") %>">Contato</a>
	</li>
	<li>
	    <a href="<%=Url.Action("index","exemplos") %>">Exemplos</a>
	</li>
	<li>
	    <a href="<%=Url.Action("upload","upload") %>">Upload</a>
	</li>
	<li>
	    <a href="<%=Url.Action("index","erro") %>">Erro</a>
	</li>
	<li>
	    <a href="<%=Url.Action("index","admin") %>">Admin</a>
	</li>
</ul>