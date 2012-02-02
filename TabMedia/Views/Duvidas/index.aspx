<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

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


        <h1>Estrutura do Aplicativo</h1>
        
        <h2>Botão Sincronizar</h2>
        <p>Tem a função de sincronizar o aplicativo. Ele informa se tem novas mídias disponíveis e, ao mesmo tempo, informa o servidor se tem novos emails para serem enviados aos médicos.</p>
        <img src="../images/ajuda_01a.jpg" width="573" height="438" />

        
        <h2>Atualizações Pendentes</h2>
        <p>Informa quais produtos têm novas mídias para serem utilizadas. Clique no botão Sincronizar para fazer o download dessas novas mídias.</p>
        <img src="../images/ajuda_01b.jpg" width="730" height="438" />

        
    </div><!--fim div conteudo--> 

</asp:Content>