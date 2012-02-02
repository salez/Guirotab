<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Controllers.GruposFormView>" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Permissoes
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<style type="text/css">
    label input {
        margin: 3px;
        position: relative;
        top: 2px;
    }
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2><%=Model.Grupo.Nome %> - Permissões</h2>
    </div>
    
    <div class="conteudo-form conteudo-permissoes">
    	<form method="post" action="">
    	    <%foreach(Controlador controlador in Model.Controladores){ %>
                
                <div class="conteudo-secundario-container">
                    <div class="conteudo-secundario">
                        <h2><%=controlador.Nome %></h2>
                        
                        <ul class="listaPermissoes">
                        <%foreach (Acao acao in controlador.Acaos)
                          { 
                               %>
                            <li>
                                <table>
                                    <tr>
                                        <td>
                                            <label style="width: auto;"><input type="checkbox" name="acoes" value="<%=acao.Id %>" <%=(acao.AcaoGrupos.Any(acaogrupo => acaogrupo.IdGrupo == Model.Grupo.Id))?"checked=\"checked\"":"" %> />
                                            <b><%=acao.Nome %></b></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="color: Gray;">
                                            <%=acao.Descricao %>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                        <%} %>
                        </ul>
                    </div>
                </div>
            <%} %>
            <div style="clear: both;" class="liSubmit">
                <input class="submit" type="submit" value="Salvar" style="width: 80px" />
            </div>
        </form>
    </div>
    
</asp:Content>