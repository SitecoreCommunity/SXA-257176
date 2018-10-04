# SXA-13922

[SXA] Variant Section marked Is Link doesn't render Link attributes

**1.** Create variant for component as follows

- `VariantDefinition` with `Field used as link target = "Link"`
  - `VariantReference` that iterates through `Items` field
    - `VariantSection` with `Tag = ""` and `Is link = "1"` and `Link attributes = "class=myclass"`
      - `VariantField` with `Tag = ""` and `Field name = "Title"`
      - `VariantField` with `Tag = ""` and `Field name = "Text"`

**2.** Render the variant against item with `Items` field that has two items selected
**3.** These items must have `Title`, `Text`, `Link (general link)` fields filled in
**4.** Check output HTML

**Actual Result:**

```html
<a href="/link1">
  <div>
    Some text
  </div>
</a>
<a href="/link2">
  <div>
    Some text
  </div>
</a>
```

**Expected Result:**

```html
<a href="/link1" class="myclass">
  <div>
    Some text
  </div>
</a>
<a href="/link2" class="myclass">
  <div>
    Some text
  </div>
</a>
```

**Note:**
Dummy `<div>` here is separate issue [SXA-13922](/SitecoreCommunity/SXA-13922).
