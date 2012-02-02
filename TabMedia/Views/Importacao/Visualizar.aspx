<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<MakroCestas.Areas.Admin.Models.Oferta>" %>
<%@ Import Namespace="MakroCestas.Areas.Admin.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visualizar
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<script type="text/javascript">

    //table sorter
    $(".tabela").tablesorter({
        widthFixed: true,
        widgets: ['zebra'],
        cssAsc: 'headerDesc',
        cssDesc: 'headerAsc',
        cssHeader: 'header'
    }); 

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
<%
    LojaRepository lojaRepository = new LojaRepository();
    
    //seleciona as lojas onde existem cestas na oferta.
    var lojas = lojaRepository.GetLojas().Where(l => l.OfertaCestas.Any(c => c.Oferta.Id == Model.Id)).OrderBy(l => l.Numero);
%>

    <div class="titulo">
        <h2>Gerenciador de Ofertas</h2>
    </div>
    
    <div class="sub-menu">
        <%=Html.ActionLink("Cadastrar nova oferta","cadastro") %>
    </div>
    
    <div class="top-tabela-cinza-cadastro conteudo-baixo" style="padding: 10px; min-height: 400px;">
        
        <div class="conteudo-secundario-container corner">
                
        <table width="996" border="0" cellpadding="0" cellspacing="3" id="tabela" class="tabela tablesorter">
            <thead>
                <tr>
                    <th style="text-align: center">Número</th>
                    <th style="text-align: center">Loja</th>
                    <th style="text-align: center">
                        Tipo E
                    </th>
                    <th style="text-align: center">
                        Tipo 1
                    </th>
                    <th style="text-align: center">
                        Tipo 2
                    </th>
                    <th style="text-align: center">
                        Tipo 3
                    </th>
                </tr>
                </thead>
            <tbody>
            
            <%foreach(var loja in lojas) {
                  var tipo1 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '1');
                  var tipo2 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '2');
                  var tipo3 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '3');
                  var tipoE = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == 'E');
                  %>

                    <tr>
                        <td style="text-align: center">
                            <%= Html.Encode(loja.Numero)%>
                        </td>
                        <td>
                            <%= Html.Encode(loja.Nome) %>
                        </td>
                        <td style="text-align: right">
                            <%= (tipoE != null) ? String.Format("{0:F2}", tipoE.Preco) : ""%>
                        </td>
                        <td style="text-align: right">
                            <%= (tipo1 != null) ? String.Format("{0:F2}", tipo1.Preco) : ""%>
                        </td>
                        <td style="text-align: right">
                            <%= (tipo2 != null) ? String.Format("{0:F2}", tipo2.Preco) : ""%>
                        </td>
                        <td style="text-align: right">
                            <%= (tipo3 != null) ? String.Format("{0:F2}", tipo3.Preco) : ""%>
                        </td>
                    </tr>

            <%} %>

                </tbody>
            </table>
        </div>
    </div>
</asp:Content>