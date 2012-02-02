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
    
	
        <h1>Menu de Navegação</h1>
    
        <h2>Atalho para tela do produto</h2>
        <p>Toque duplo na tela seguido de arrasto para baixo volta para a tela do produto.</p>
        <img src="../images/ajuda_02a.jpg" width="730" height="375" />
        
        <h2>Barra de Navegação</h2>
        <p>A barra de navegação deve ser acessada com toque duplo na tela.</p>
        <img src="../images/ajuda_02b.jpg" width="515" height="319" />
        
        <h2>Seleção de página</h2>
        <p>Exibe todas as páginas disponíveis no VA ou na Lâmina selecionada. Clique sobre a página que quer abrir.</p>
        <img src="../images/ajuda_02c.jpg" width="730" height="247" />
        
        <h2>Página do produto</h2>
        <p>Exibe todo material relacionado ao produto: VA, Lâminas e Arquivos (Esses arquivos podem ser Artigos Científicos, Estudos, Notícias, Bulas e tudo mais que pode ser útil para a apresentação).</p>
        <img src="../images/ajuda_02d.jpg" width="330" height="247" />

        
        <h2>Produtos</h2>
        <p>Exibe uma lista com todos os produtos disponíveis. Permitindo uma navegação rápida para outro produto.</p>
        <img src="../images/ajuda_02e.jpg" width="331" height="247" />
        
        <h2>Comentários</h2>
        <p>Cria um canal entre a Agência e os Gerentes de Produto e Marketing. Exibindo históricos e permitindo novos comentários.</p>
        <img src="../images/ajuda_02f.jpg" width="336" height="247" />
        
        <h2>Aprovar/Reprovar</h2>
        <p>Permite a agência enviar um arquivo para aprovação, assim como os gerentes podem aprovar ou reprovar um arquivo.</p>
        <img src="../images/ajuda_02g.jpg" width="332" height="247" />
        
        <h2>Home</h2>
        <p>Ao clicar nesse botão, você será direcionado para a página inicial do aplicativo.</p>
        <img src="../images/ajuda_02h.jpg" width="328" height="252" />
        
        <!--
        <h2>Voltar ao Visilab</h2>
        <p>Clique sobre esse botão para voltar ao Visilab automaticamente.</p> 
        <img src="../images/ajuda_02i.jpg" width="332" height="254" />
		-->

    </div><!--fim div conteudo--> 
    


    

</asp:Content>