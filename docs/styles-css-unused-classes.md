# Auditoria de classes não utilizadas (`WEB/wwwroot/css/styles.css`)

Data da auditoria: 2026-03-28

## Escopo

- Arquivo auditado: `WEB/wwwroot/css/styles.css`
- Procura de uso em arquivos do projeto (`.cshtml`, `.cs`, `.js`, `.css`, `.html`, `.ts`, `.tsx`, `.jsx`, `.json`, `.md`), excluindo bibliotecas de terceiros (`WEB/wwwroot/lib/**`) e o próprio `styles.css`.

## Metodologia

1. Extraí os seletores de classe definidos em `styles.css`.
2. Para cada classe, fiz busca por token exato no restante do projeto.
3. Classifiquei como **não utilizada** quando não havia ocorrência fora do próprio `styles.css`.

## Resultado

As classes sem uso identificadas foram removidas de `styles.css`:

- `.status-pill`
- `.status-ok`
- `.status-away`
- `.status-off`

## Observações importantes

- Essas classes parecem ser um bloco coeso para "pílulas de status" (estados visualmente diferentes).
- Não havia referência a elas nas Views Razor atuais nem em scripts de página no momento da auditoria.
- Como são classes semânticas e agrupadas, pode ser código preparado para uma funcionalidade futura.

## Recomendações

1. Classes removidas para reduzir CSS morto.
2. Se essas classes voltarem a ser necessárias, reintroduzir no contexto da feature que consumir os estados visuais.
3. Após remoção, rodar uma regressão visual básica das telas de bovinos/suínos para confirmar ausência de impacto.
