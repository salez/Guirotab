<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Ajuda>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Ajuda
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
    	<h4></h4>
        <h2>Ajuda</h2>
        <h4></h4>
    </div>
    
    

    
    <div id="menuAjuda">
		<ul>
		<!--<li><a class="titulo">Plataforma TabMedia</a></li>-->
		<li><a href="<%=Url.Action("index","ajuda") %>" >Estrutura do Aplicativo</a></li>
		<li><a href="<%=Url.Action("menu","ajuda") %>" >Menu de Navegação</a></li>
		<li><a href="<%=Url.Action("paginas","ajuda") %>" >Avanço de páginas</a></li>
		<li><a href="<%=Url.Action("campanhas","ajuda") %>" >Campanhas em HTML</a></li>
		<li><a href="<%=Url.Action("suporte","ajuda") %>" >Suporte Técnico</a></li>
		</ul>
    </div><!--fim div menu--> 
    
    <div id="conteudoAjuda">

        <h1>Suporte Técnico</h1>
        
        <p>Para solucionar dúvidas em relação à utilização da plataforma, entre em contato com nosso suporte.</p>
        
        <p>suporte@tabfarma.com.br</p>
        
    </div><!--fim div conteudo--> 
    


    

</asp:Content>