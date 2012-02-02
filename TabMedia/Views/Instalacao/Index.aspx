<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Instalação</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">
    
    	<h2 style="margin-top: 0;">Habilitar Aplicativo</h2>
        <br /><br /><br />
        Para habilitar o aplicativo é necessário acessar o sistema via iPad e clicar em Instalar.

        <br /><br /><a class="botao" href="itms-services://?action=download-manifest&url=http://tabmedia.com.br/nycomed/2.0.1/manifest.plist" target="_blank">Instalar</a>

        <br /><br />

    </div>

</asp:Content>