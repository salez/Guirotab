<%@ Page Language="C#" ContentType="text/css" Inherits="System.Web.Mvc.ViewPage" %>
<% Models.CustomConfig customConfig = (new Models.CustomConfigRepository()).GetConfig(); %>

body{
    background: <%=customConfig.AdminCorFundo %>;
}

a
{
    color: <%=customConfig.AdminCorLinksFonte %>;
    color:#1e7177 !important;
}

a:hover{
    color: <%=customConfig.AdminCorLinksHoverFonte %>;
    color:#269ca3 !important;
}

.menu a{
    color: <%=customConfig.AdminCorLinksFonte %>;
    color:#fff !important;
}

.menu a:hover{
    color: <%=customConfig.AdminCorLinksHoverFonte %>;
    color:#fff !important;
}


.login-body{
    background: url(<%=Util.Sistema.SiteUrl %>/images/fundo_login.jpg) repeat-x top #ffffff !important;
}

div.login{
    background-color: <%=customConfig.AdminCorLoginFundo %>;
    color: <%=customConfig.AdminCorLoginFonte %>;
    /*height: 280px;*/
}

div.login a{
    color: <%=customConfig.AdminCorLoginFonte %>;
}

.hLogo{
    /*height: 150px;*/
	background:url(<%=Util.Sistema.SiteUrl %>/Images/<%=customConfig.AdminImagemLogoLogin%>) center center no-repeat;
}

.topo{
    /*height: 100px;*/
}

.topo h2{
    background:url(<%=Util.Sistema.SiteUrl %>/Images/<%=customConfig.AdminImagemLogo%>) 0 0 no-repeat;
}

.topo_login h2{
    background:url(<%=Util.Sistema.SiteUrl %>/Images/<%=customConfig.AdminImagemLogo%>) top center no-repeat;
}

.menutop{
    /*width: 300px;*/
    background-color: <%=customConfig.AdminCorMenuTopoFundo %>;
    color: <%=customConfig.AdminCorMenuTopoFonte %>;
}

.menutop a.botao{
    background-color: <%=customConfig.AdminCorMenuTopoBotoesFundo%>;
	color: <%=customConfig.AdminCorMenuTopoBotoesFonte%>;
}

.menutop a:hover.botao{
    background-color: <%=customConfig.AdminCorMenuTopoBotoesHoverFundo%>;
	color: <%=customConfig.AdminCorMenuTopoBotoesHoverFonte %>;
}





.menu2 .menutop{
    /*width: 300px;*/
    background-color: <%=customConfig.AdminCorMenuTopoFundo %>;
    color: <%=customConfig.AdminCorMenuTopoFonte %>;
}

.menu2 .menutop a.botao{
    background-color: <%=customConfig.AdminCorMenuTopoBotoesFundo%>;
	color: <%=customConfig.AdminCorMenuTopoBotoesFonte%>;
}

.menu2 .menutop a:hover.botao{
    background-color: <%=customConfig.AdminCorMenuTopoBotoesHoverFundo%>;
	color: <%=customConfig.AdminCorMenuTopoBotoesHoverFonte %>;
}






.menu{
    background-color: <%=customConfig.AdminCorMenuFundo %>;
}

.menu a{
    color: <%=customConfig.AdminCorMenuFonte %>;
}

.menu a:hover{
    color: <%=customConfig.AdminCorMenuHoverFonte %>;
}





.menu2{
    background-color: <%=customConfig.AdminCorMenuFundo %>;
}

.menu2 a{
    color: <%=customConfig.AdminCorMenuFonte %>;
}

.menu2 a:hover{
    color: <%=customConfig.AdminCorMenuHoverFonte %>;
}






<% if (customConfig.AdminOrientacaoMenu == "vertical") { %>

    .menu {
        /*padding: 10px 0;
        width: 150px;
        height: auto;
        margin-right: 10px;*/
    }
    
    .menu a{
	    /*text-decoration: none;
	    padding: 5px 0;
	    display: block;
	    text-align: center;*/
    }
    
    .menu a:hover{
        background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu .submenu{
        /*display: block;*/
    }
    



    .menu2 {
        /*padding: 10px 0;
        width: 150px;
        height: auto;
        margin-right: 10px;*/
    }
    
    .menu2 a{
	    /*text-decoration: none;
	    padding: 5px 0;
	    display: block;
	    text-align: center;*/
    }
    
    .menu2 a:hover{
        background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu2 .submenu{
        /*display: block;*/
    }
 
 
 
    
    .conteudo{
	    /*width:835px;*/
    }
    

<% } else { %>

    .menu {
        /*width: 977px;*/
	    height: 40px;
	    margin: 0 auto;
	    padding: 0 0 0 20px;
	}
	
	.menu a{
	    text-decoration: none;
	    padding: 0 14px 0 16px;
	    display: inline-block;
	    height: 38px;
	    line-height: 38px;
	    float: left;
	    text-align: left;
        /*overflow:hidden;*/
    }
    
    .menu a:hover{
    	background: #336061;
    }
    
    .menu a.ativo{
    	background: url(/Images/btn_ativo.jpg) !important;
    }
    
	.menu a:hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu a.hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }

    .menu .submenu li{
        background-color: white;
    }

<%--    
    .menu .submenu li{
        background-color: <%=customConfig.AdminCorMenuFundo %>;
    }--%>
    
    .menu .submenu a{
	    background-color: <%=customConfig.AdminCorMenuFundo %>;
    }
    
    .menu .submenu a:hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu .submenu{
        display: block;
        position: absolute;
    }







    .menu2 {
        /*width: 977px;*/
	    height: 134px;
	    padding: 0;
        overflow:hidden;
        display:inline-block;
	}
	
	.menu2 a{
	    text-decoration: none;
	    padding: 0 14px;
	    display: inline-block;
	    height: 38px;
	    line-height: 40px;
	    float: left;
	    text-align: center;
    }
    
    .menu2 a:hover{
    	color:#25bfc7;
    }
    
	.menu2 a:hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu2 a.hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu2 .submenu li{
        background-color: <%=customConfig.AdminCorMenuFundo %>;
    }
    
    .menu2 .submenu a{
	    background-color: <%=customConfig.AdminCorMenuFundo %>;
    }
    
    .menu2 .submenu a:hover{
	    background-color: <%=customConfig.AdminCorMenuHoverFundo %>;
    }
    
    .menu2 .submenu{
        display: block;
        position: absolute;
    }







    
    .conteudo{
        margin-top: 35px;
	    /*width:997px;*/
    }
	
<%} %>

.conteudo{
    background: <%=customConfig.AdminCorConteudoFundo %>;
    color: <%=customConfig.AdminCorConteudoFonte %>;
}

.conteudo .titulo{
    background-color: <%=customConfig.AdminCorTituloFundo %>;
    color: <%=customConfig.AdminCorTituloFonte %>;
}

.botoes a{
    background-color: <%=customConfig.AdminCorBotoesFundo %>;
    color: <%=customConfig.AdminCorBotoesFonte %>;
}

.botoes a:hover{
    background-color: <%=customConfig.AdminCorBotoesHoverFundo %>;
    color: <%=customConfig.AdminCorBotoesHoverFonte%>;
}

a.botao{
    background-color: <%=customConfig.AdminCorBotoesFundo %>;
    color: <%=customConfig.AdminCorBotoesFonte %>;
}

a.botao:hover{
    background-color: <%=customConfig.AdminCorBotoesHoverFundo %>;
    color: <%=customConfig.AdminCorBotoesHoverFonte%>;
}

.jqTransformSubmit{
    background-color: <%=customConfig.AdminCorBotoesFundo%>;
    margin:0;
}

.jqTransformSubmit input{
    color: <%=customConfig.AdminCorBotoesFonte%>;
}

.jqTransformSubmit_hover{
    background-color: <%=customConfig.AdminCorBotoesHoverFundo%>;
}

.jqTransformSubmit_hover input{
    color: <%=customConfig.AdminCorBotoesHoverFonte%>;
}

button.jqTransformButton{
    background-color: <%=customConfig.AdminCorBotoesFundo%>;
    color: <%=customConfig.AdminCorBotoesFonte%>;
}

button.jqTransformButton_hover{
    background-color: <%=customConfig.AdminCorBotoesHoverFundo%>;
    color: <%=customConfig.AdminCorBotoesHoverFonte%>;
}

.inferior a.botao{
    background: <%=customConfig.AdminCorRodapeBotoesFundo %>;
    color: <%=customConfig.AdminCorRodapeBotoesFonte %>;
}

.inferior a:hover.botao{
    background: <%=customConfig.AdminCorRodapeBotoesHoverFundo %>;
    color: <%=customConfig.AdminCorRodapeBotoesHoverFonte %>;
}

<%--.tabela-container{
    background-color: <%=customConfig.AdminCorTabelaFundo %>;
}

.conteudo-baixo .tabela .odd { 
    background-color: <%=customConfig.AdminCorTabelaLinha1Fundo %>; 
    color: <%=customConfig.AdminCorTabelaLinha1Fonte%>;
}	

.conteudo-baixo .tabela .even { 
    background-color: <%=customConfig.AdminCorTabelaLinha2Fundo %>;
    color: <%=customConfig.AdminCorTabelaLinha2Fonte %>;
}

.tabela tr th.headerAsc{
    color: <%=customConfig.AdminCorTabelaHeaderSelecionadoFonte %>;
    background: url(<%=Util.Sistema.SiteUrl %>/areas/admin/images/<%=customConfig.AdminImagemTabelaHeaderSetaCima %>) left center no-repeat;
}

.tabela tr th.headerDesc{
    color: <%=customConfig.AdminCorTabelaHeaderSelecionadoFonte %>;
    background: url(<%=Util.Sistema.SiteUrl %>/areas/admin/images/<%=customConfig.AdminImagemTabelaHeaderSetaBaixo %>) left center no-repeat;
}

.conteudo-secundario-container{
    background-color: <%=customConfig.AdminCorConteudoSecundarioFundo %>;
    color: <%=customConfig.AdminCorConteudoSecundarioFonte %>;
}--%>