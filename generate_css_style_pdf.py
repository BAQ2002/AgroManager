import re
from pathlib import Path

ROOT = Path('/workspace/AgroManager')
css_files = [p for p in ROOT.rglob('*.css') if '/lib/' not in p.as_posix()]
css_files = sorted(css_files)

prop_explanations = {
    'align-items': 'Define o alinhamento dos itens no eixo transversal em um container flex/grid.',
    'appearance': 'Controla a aparência nativa de controles de formulário.',
    'background': 'Define o plano de fundo (cor, imagem ou gradiente).',
    'background-color': 'Define apenas a cor de fundo do elemento.',
    'border': 'Atalho para definir largura, estilo e cor da borda.',
    'border-bottom': 'Define a borda inferior do elemento.',
    'border-bottom-color': 'Define a cor da borda inferior.',
    'border-collapse': 'Define como as bordas de tabelas são renderizadas.',
    'border-color': 'Define a cor das bordas.',
    'border-radius': 'Arredonda os cantos do elemento.',
    'border-right': 'Define a borda direita do elemento.',
    'border-top': 'Define a borda superior do elemento.',
    'bottom': 'Posiciona o elemento em relação à base do container de referência.',
    'box-shadow': 'Aplica sombra ao redor do elemento.',
    'box-sizing': 'Define como largura/altura consideram padding e border.',
    'clip': 'Recorta a área visível do elemento (técnica antiga, comum em acessibilidade visual).',
    'color': 'Define a cor do texto.',
    'content': 'Define conteúdo gerado em pseudo-elementos (::before/::after).',
    'cursor': 'Define o tipo de cursor exibido ao passar o mouse.',
    'display': 'Define o tipo de caixa e comportamento de layout do elemento.',
    'flex': 'Atalho para crescimento, encolhimento e base de itens flex.',
    'flex-direction': 'Define a direção principal dos itens em container flex.',
    'font-family': 'Define a família tipográfica usada no texto.',
    'font-size': 'Define o tamanho da fonte.',
    'font-variant-numeric': 'Controla variantes numéricas da fonte (ex.: números tabulares).',
    'font-weight': 'Define a espessura da fonte.',
    'gap': 'Define espaçamento entre itens de flex/grid.',
    'grid-template-columns': 'Define as colunas de um grid.',
    'height': 'Define a altura do elemento.',
    'justify-content': 'Alinha/distribui itens no eixo principal em flex/grid.',
    'left': 'Posiciona o elemento em relação à esquerda do container.',
    'letter-spacing': 'Define o espaçamento entre caracteres.',
    'line-height': 'Define a altura da linha de texto.',
    'margin': 'Define margens externas do elemento.',
    'margin-bottom': 'Define a margem inferior.',
    'margin-left': 'Define a margem esquerda.',
    'margin-top': 'Define a margem superior.',
    'max-height': 'Define altura máxima do elemento.',
    'max-width': 'Define largura máxima do elemento.',
    'min-height': 'Define altura mínima do elemento.',
    'min-width': 'Define largura mínima do elemento.',
    'opacity': 'Define o nível de transparência.',
    'outline': 'Desenha contorno externo ao elemento (não ocupa espaço).',
    'outline-offset': 'Define distância entre outline e a borda.',
    'overflow': 'Controla como conteúdo excedente é exibido.',
    'padding': 'Define espaçamento interno do elemento.',
    'padding-right': 'Define espaçamento interno à direita.',
    'pointer-events': 'Controla se o elemento responde a eventos de ponteiro.',
    'position': 'Define o método de posicionamento (static, relative, absolute, etc.).',
    'text-align': 'Define alinhamento horizontal do texto.',
    'text-decoration': 'Define decoração do texto (sublinhado etc.).',
    'text-transform': 'Transforma o texto (maiúsculas/minúsculas).',
    'top': 'Posiciona o elemento em relação ao topo do container.',
    'transform': 'Aplica transformações geométricas (translação, rotação, escala).',
    'transition': 'Define transição suave entre mudanças de propriedades.',
    'user-select': 'Define se o usuário pode selecionar texto/conteúdo.',
    'white-space': 'Controla quebra e colapso de espaços em branco.',
    'width': 'Define largura do elemento.',
    'word-break': 'Controla quebra de palavras longas.',
    'z-index': 'Define ordem de empilhamento de elementos posicionados.',
}

def explain(prop):
    p = prop.strip().lower()
    if p in prop_explanations:
        return prop_explanations[p]
    if p.startswith('--'):
        return 'Define uma propriedade customizada (variável CSS) reutilizável.'
    return 'Define um aspecto visual/comportamental específico do elemento.'


def strip_comments(s):
    return re.sub(r'/\*.*?\*/', '', s, flags=re.S)


def extract_rules(css):
    css = strip_comments(css)
    rules = []
    i = 0
    n = len(css)
    stack = []
    while i < n:
        if css.startswith('@media', i):
            j = css.find('{', i)
            if j == -1:
                break
            cond = css[i:j].strip()
            stack.append(cond)
            i = j + 1
            continue
        if css[i] == '}':
            if stack:
                stack.pop()
            i += 1
            continue
        j = css.find('{', i)
        if j == -1:
            break
        selector = css[i:j].strip()
        if not selector:
            i = j + 1
            continue
        k = j + 1
        depth = 1
        while k < n and depth:
            if css[k] == '{':
                depth += 1
            elif css[k] == '}':
                depth -= 1
            k += 1
        body = css[j+1:k-1].strip()
        if selector.startswith('@'):
            i = k
            continue
        context = ' > '.join(stack)
        rules.append((context, selector, body))
        i = k
    return rules


def parse_decls(body):
    out = []
    for part in body.split(';'):
        if ':' in part:
            p,v = part.split(':',1)
            p=p.strip(); v=v.strip()
            if p and v:
                out.append((p,v))
    return out

lines = []
lines.append('Guia de Atributos CSS do Projeto AgroManager')
lines.append('')
lines.append('Este documento lista os seletores e explica cada atributo style encontrado nos arquivos CSS do projeto (exceto bibliotecas de terceiros em /lib).')
lines.append('')

for css_file in css_files:
    rel = css_file.relative_to(ROOT).as_posix()
    content = css_file.read_text(encoding='utf-8-sig')
    rules = extract_rules(content)
    lines.append(f'Arquivo: {rel}')
    if not rules:
        lines.append('  (Nenhuma regra detectada)')
        lines.append('')
        continue
    for context, selector, body in rules:
        header = selector
        if context:
            header = f'{context} :: {selector}'
        lines.append(f'Seletor: {header}')
        decls = parse_decls(body)
        for p,v in decls:
            lines.append(f'  - {p}: {v}')
            lines.append(f'    -> {explain(p)}')
        lines.append('')

# PDF writer
PAGE_W, PAGE_H = 595, 842
MARGIN = 40
FONT_SIZE = 10
LEADING = 14
usable_chars = 95

wrapped = []
for line in lines:
    if not line:
        wrapped.append('')
        continue
    prefix = ''
    text = line
    while len(text) > usable_chars:
        cut = text.rfind(' ', 0, usable_chars)
        if cut <= 0:
            cut = usable_chars
        wrapped.append(prefix + text[:cut])
        text = text[cut:].lstrip()
        prefix = '    '
    wrapped.append(prefix + text)

pages = []
cur = []
y_lines = int((PAGE_H - 2*MARGIN) / LEADING)
for l in wrapped:
    if len(cur) >= y_lines:
        pages.append(cur); cur=[]
    cur.append(l)
if cur:
    pages.append(cur)

objects = []

def add_obj(data: bytes):
    objects.append(data)
    return len(objects)

font_obj = add_obj(b'<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>')
page_objs = []
content_objs = []

for page in pages:
    content_lines = ['BT', f'/F1 {FONT_SIZE} Tf', f'{MARGIN} {PAGE_H - MARGIN} Td', f'{LEADING} TL']
    first=True
    for ln in page:
        safe = ln.replace('\\','\\\\').replace('(','\\(').replace(')','\\)')
        if first:
            content_lines.append(f'({safe}) Tj')
            first=False
        else:
            content_lines.append('T*')
            content_lines.append(f'({safe}) Tj')
    content_lines.append('ET')
    stream = '\n'.join(content_lines).encode('latin-1','replace')
    cont = f'<< /Length {len(stream)} >>\nstream\n'.encode()+stream+b'\nendstream'
    cobj = add_obj(cont)
    content_objs.append(cobj)

pages_kids_refs = []
for cobj in content_objs:
    page_dict = f'<< /Type /Page /Parent 0 0 R /MediaBox [0 0 {PAGE_W} {PAGE_H}] /Resources << /Font << /F1 {font_obj} 0 R >> >> /Contents {cobj} 0 R >>'.encode()
    pobj = add_obj(page_dict)
    page_objs.append(pobj)
    pages_kids_refs.append(f'{pobj} 0 R')

kids=' '.join(pages_kids_refs)
pages_obj_idx = add_obj(f'<< /Type /Pages /Kids [ {kids} ] /Count {len(page_objs)} >>'.encode())
for p in page_objs:
    objects[p-1] = objects[p-1].replace(b'/Parent 0 0 R', f'/Parent {pages_obj_idx} 0 R'.encode())

catalog_obj = add_obj(f'<< /Type /Catalog /Pages {pages_obj_idx} 0 R >>'.encode())

pdf = bytearray(b'%PDF-1.4\n')
offsets = [0]
for i,obj in enumerate(objects, start=1):
    offsets.append(len(pdf))
    pdf += f'{i} 0 obj\n'.encode() + obj + b'\nendobj\n'

xref_pos = len(pdf)
pdf += f'xref\n0 {len(objects)+1}\n'.encode()
pdf += b'0000000000 65535 f \n'
for i in range(1, len(objects)+1):
    pdf += f'{offsets[i]:010d} 00000 n \n'.encode()
pdf += f'trailer\n<< /Size {len(objects)+1} /Root {catalog_obj} 0 R >>\nstartxref\n{xref_pos}\n%%EOF'.encode()

out = ROOT / 'docs' / 'guia-atributos-css.pdf'
out.parent.mkdir(parents=True, exist_ok=True)
out.write_bytes(pdf)
print(f'PDF gerado em: {out}')
print(f'Arquivos CSS processados: {len(css_files)}')
