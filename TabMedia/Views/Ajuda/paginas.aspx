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


        <h1>Avanço de Páginas</h1>
        
        <h2>Arrasto com um dedo</h2>
        <p>Da direita para a esquerda exibe a próxima tela. Da esquerda para a direita exibe a tela anterior.</p>
        <img src="../images/ajuda_03a.jpg" width="426" height="363" />
        
        <h2>Arrasto com dois dedos</h2>
        <p>Da direita para a esquerda exibe o próximo produto. Da esquerda para a direita exibe o produto anterior.</p>
		<img src="../images/ajuda_03b.jpg" width="426" height="346" />
        
        <h2>Dois toques na tela</h2>
        <p>Exibe a barra de menu.</p>
		<img src="../images/ajuda_03c.jpg" width="426" height="346" />
        
        
    </div><!--fim div conteudo--> 
    


    

</asp:Content>