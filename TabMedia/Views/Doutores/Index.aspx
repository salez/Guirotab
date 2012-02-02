<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Agências
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function carregaDoutores() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetDoutores") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#tabelaVasPendentes').hide();
                        $('#divSemVaPendente').show();
                    } else {

                        $('#conteudoTabela').html(data);

                        $('#tabelaVasPendentes').show();
                        $('#divSemVaPendente').hide();
                    }

                    updateTableSort();

                }
            });

        }

        $(function () {

            carregaDoutores();

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Doutores</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">

        <div class="tabela-container">
            
            <div id="divSemVaPendente" style="padding: 10px 10px 0pt; display: none;">Nenhum doutor encontrado</div>

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
                        <th>
                            CRM
                        </th>
                        <th>
                            CRMUF
                        </th>
                        <th>
                            Especialidades
                        </th>
                    </tr>
                </thead>
                <tbody id="conteudoTabela">
                    <tr style="background-color: White;">
                        <th colspan="5">
                            &nbsp;
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>

    </div>

</asp:Content>