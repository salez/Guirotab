<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Territórios
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function carregaTerritorios() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetTerritorios") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#tabelaVasPendentes').hide();
                        $('#divSemVaPendente').show();
                    } else {

                        $('#vasPendentes').html(data);

                        $('#tabelaVasPendentes').show();
                        $('#divSemVaPendente').hide();
                    }

                    //atualiza tablesorter
                    $(".tabela").trigger("update");

                }
            });

        }

        $(function () {

            carregaTerritorios();

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Territórios</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">

        <div class="tabela-container">
            
            <div id="divSemVaPendente" style="padding: 10px 10px 0pt; display: none;">Nenhum território encontrada</div>

            <div class="carregando">Carregando...</div>

            <table id="tabelaVasPendentes" class="tabela tablesorter" style="display: none;">
                <thead>
                    <tr>
                        <th>
                            Território
                        </th>
                        <th>
                            Nome
                        </th>
                        <%--<th>
                            Email
                        </th>--%>
                        <th>
                            Linha
                        </th>
                        <th>
                            Status
                        </th>
                    </tr>
                </thead>
                <tbody id="vasPendentes">
                    <tr style="background-color: White;">
                        <td colspan="6">
                            <%--html/GetTerritorios.htm--%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

    </div>

</asp:Content>