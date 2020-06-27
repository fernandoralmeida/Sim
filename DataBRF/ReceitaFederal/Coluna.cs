using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Data.OnLine.ReceitaFederal
{
    internal enum Coluna
    {

        RazaoSocial,
        NomeFantasia,
        AtividadeEconomicaPrimaria,
        AtividadesEconomicasSecundarias, // AGORA BUSCA TODAS AS ATIVIDADES SECUNDARIAS E NAO SOMENTE UMA

        NumeroDaInscricao,
        MatrizFilial,
        NaturezaJuridica,

        SituacaoCadastral,
        DataSituacaoCadastral,
        MotivoSituacaoCadastral,

        Endereco,
        Numero,
        Bairro,
        Cidade,
        UF,
        Complemento,
        CEP,

        // PROPRIEDADES ADICIONADAS
        Telefone,
        Email,
        DataAbertura,
        EnteFederativoResponsavel,
        SituacaoEspecial,
        DataSituacaoEspecial,
        Cnae

    }
}
