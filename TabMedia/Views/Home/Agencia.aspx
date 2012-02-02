<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Home</h2>
    </div>

    <div class="conteudo-baixo vas-visualizar">

         <h2 style="margin-top: 0; float: none;">Instalar Aplicativo</h2>

        Para instalar a nova versão do aplicativo TabMedia, clique no botão Instalar no topo dessa página e efetue login com seu usuário e senha.

        <br /><br />
        
        <a class="botao" href="itms-services://?action=download-manifest&url=http://tabmedia.com.br/nycomed/app/2.0.1/manifest.plist" target="_blank">Instalar</a> 
        <%--<a class="botao" href="emspresentations://update?territory=<%=Sessao.Site.RetornaUsuario().TerritorioSimulado %>" target="_blank">Habilitar</a>--%>

        <br /><br />

    </div>
    
</asp:Content>