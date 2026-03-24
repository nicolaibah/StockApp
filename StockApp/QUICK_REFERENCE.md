# The High-Stakes Atelier: Quick Reference

## 🎨 Design System Quick Start

### Color Tokens

```
🌙 Dark Blues (Surface):
--surface                 #061423   (Base)
--surface-container-low   #0f1c2c   (Secondary)
--surface-container       #132030   (Primary)
--surface-container-high  #1e2b3b   (Active)
--surface-container-highest #283646 (Floating)
--surface-container-lowest  #020f1e (Recessed)

💚 Growth Green:
--secondary               #7dffa2   (Success, Growth)

💙 Primary Blue:
--primary                 #bbc6e2   (Main accent)
--primary-container       #0f1a2e   (Gradient)

❌ Error Red:
--error                   #ffb4ab   (Loss, Error)
```

### Typography

```
📖 Manrope (Headlines):
.display-lg              3.5rem   (Portfolio values)
.display-md              2.75rem  (Major headers)
.display-sm              2.25rem  (Subsections)
.headline-md             1.75rem  (Section headers)
.headline-sm             1.5rem   (Subsection titles)

📝 Inter (Body):
.body-md                 0.875rem (Descriptions, UI text)

📊 Work Sans (Data):
.label-md                0.75rem  (Tickers, numbers)
```

### Spacing Scale

```
.spacing-2 / --spacing-2     0.5rem    (Micro)
.spacing-4 / --spacing-4     0.9rem    (Small)
.spacing-6 / --spacing-6     1.5rem    (Standard)
.spacing-8 / --spacing-8     2rem      (Medium)
.spacing-10 / --spacing-10   2.25rem   (Large - breathing room)
.spacing-12 / --spacing-12   2.75rem   (Extra large - breathing room)
```

---

## 🧩 Component Patterns

### Buttons

```html
<!-- Primary (Green) -->
<button class="btn btn-primary">Action</button>

<!-- Tertiary (No background, hover shift) -->
<button class="btn btn-tertiary">Secondary</button>

<!-- Ghost (15% opacity outline) -->
<button class="btn btn-ghost">Minimal</button>
```

### Cards

```html
<!-- Standard Layer -->
<div class="card">Content</div>

<!-- Elevated Floating -->
<div class="card card-elevated">Floating content</div>

<!-- Custom Layer -->
<div class="card surface-container-high">Active content</div>
```

### Tables

```html
<table class="design-table">
    <thead>
        <tr>
            <th>Header</th>
        </tr>
    </thead>
    <tbody>
        <tr class="surface-container-lowest">
            <td class="data-cell">MSFT</td>
        </tr>
    </tbody>
</table>
```

### Glassmorphic Panels

```html
<div class="glass-panel">
    <h3 class="headline-sm">Title</h3>
    <p>Content with 60% opacity + 20px blur backdrop</p>
</div>
```

### Input Fields

```html
<!-- Standard with ghost border on focus -->
<input type="text" placeholder="Search..." />

<!-- Focus shows: green outline + soft glow -->
```

---

## 📐 Layout Patterns

### Asymmetric Hero

```html
<div class="flex-between mb-12" style="align-items: flex-start;">
    <div>
        <h1 class="display-md">Left: Large Title</h1>
        <p class="body-md">Left: Description</p>
    </div>
    <div class="gap-breathing flex-column" style="text-align: right;">
        <div class="display-sm text-secondary">Right: Number</div>
        <div class="label-md text-primary">Right: Label</div>
    </div>
</div>
```

### Layer Stack (Depth through color)

```html
<div class="layer-low">                    ← surface-container-low
    <div class="layer">                    ← surface-container
        <div class="layer-lowest">         ← surface-container-lowest
            Recessed content
        </div>
    </div>
</div>
```

### No-Line Divider (Spacing instead of borders)

```html
<div class="surface-container-low p-8 mb-4">Item 1</div>
<div class="surface-container-low p-8">Item 2</div>
<!-- Spacing + color = visual separation, no lines -->
```

### Breathing Room

```html
<div class="breathing-lg">
    <!-- padding: 2.75rem on all sides -->
    <!-- Creates luxury feel through generous space -->
</div>
```

---

## 🎯 Common Use Cases

### Leaderboard with Current Player Highlighting

```html
@foreach (var player in players)
{
    <tr class="@(player.IsCurrent ? "surface-container-high" : "surface-container-lowest")">
        <td class="data-cell">@player.Rank</td>
        <td>@player.Name</td>
        <td class="data-cell text-secondary">@player.Value</td>
    </tr>
}
```

### Growth vs Loss Indication

```html
<!-- Growth: Green -->
<span class="text-secondary">+12.5% (Growth)</span>

<!-- Loss: Red -->
<span class="text-error">-8.3% (Loss)</span>
```

### Time Range Button Selection

```html
<button class="@(selected == "1d" ? "btn btn-primary" : "btn btn-tertiary")">
    1 dag
</button>
```

### Calculated Display with Gradient

```html
<div class="surface-container-low p-6">
    <div class="label-md text-secondary mb-2">Price per unit</div>
    <div class="display-sm gradient-primary-text">@price</div>
</div>
```

### Modal with Glass Effect

```html
<div class="glass-panel-overlay">
    <div class="glass-panel">
        <h2 class="headline-md mb-8">Modal Title</h2>
        <form><!-- Form content --></form>
    </div>
</div>
```

---

## 🚫 Anti-Patterns to Avoid

```html
<!-- ❌ DON'T: Use borders for separation -->
<div style="border-bottom: 1px solid #ccc;">Item</div>

<!-- ✅ DO: Use spacing + color -->
<div class="surface-container p-8 mb-4">Item</div>

<!-- ❌ DON'T: Pure black backgrounds -->
<div style="background: #000;">Dark</div>

<!-- ✅ DO: Use surface color -->
<div class="surface">Dark</div>

<!-- ❌ DON'T: High opacity shadows -->
<div style="box-shadow: 0 4px 8px rgba(0,0,0,0.3);">Box</div>

<!-- ✅ DO: Use ambient shadow -->
<div style="box-shadow: var(--shadow-ambient);">Box</div>

<!-- ❌ DON'T: Hardcode colors -->
<button style="background-color: #7dffa2;">Click</button>

<!-- ✅ DO: Use design tokens -->
<button class="btn btn-primary">Click</button>
```

---

## 🔧 Utility Classes Cheat Sheet

### Flexbox
```
.flex-center        /* center all items */
.flex-between       /* space-between horizontally */
.flex-column        /* flex-direction: column */

.gap-2 .gap-4 .gap-6 .gap-8
.gap-breathing      /* 2.25rem gap */
```

### Colors
```
.text-primary       /* #bbc6e2 */
.text-secondary     /* #7dffa2 */
.text-error         /* #ffb4ab */
.text-surface       /* #d6e4f9 */
```

### Spacing
```
.p-4 .p-6 .p-8          /* Padding */
.px-8                   /* Horizontal padding */
.py-8                   /* Vertical padding */
.m-4 .mb-4 .mt-4 .mx-4  /* Margin variants */
```

### Surfaces
```
.surface                    /* Base layer */
.surface-container-low      /* Secondary */
.surface-container          /* Primary */
.surface-container-high     /* Active */
.surface-container-highest  /* Floating */
.surface-container-lowest   /* Recessed */
```

### Effects
```
.glass-panel            /* Glassmorphism overlay */
.gradient-primary       /* Blue gradient background */
.gradient-primary-text  /* Gradient text effect */
.card                   /* Standard card styling */
.card-elevated          /* Floating card */
```

---

## 📱 Responsive Considerations

The design system uses:
- **Flexible layouts** (flexbox, CSS variables)
- **Relative sizing** (rem, not px)
- **Mobile-first approach**
- **Breathing room scales** with content

No specific breakpoints needed - system flows naturally.

---

## ♿ Accessibility

✅ **WCAG 2.1 AA Compliant**
- Color contrast ratios meet standards
- Focus indicators on all interactive elements
- Semantic HTML throughout
- Labels on form fields
- Proper heading hierarchy

---

## 📚 File Locations

```
Design System Files:
├── wwwroot/css/app.css              ← All CSS tokens & classes
├── wwwroot/index.html               ← Font imports
├── Components/DesignButton.razor     ← Button component
├── Components/DesignCard.razor       ← Card component
├── Components/DesignDataTable.razor  ← Table component
└── Components/GlassmorphicPanel.razor ← Modal component

Implementation Examples:
├── Pages/Home.razor                 ← Asymmetric layout
├── Pages/Game.razor                 ← Layered design
└── Dialogs/TransactionDialog.razor  ← Glassmorphism

Documentation:
├── Design.md                        ← Original spec
├── DESIGN_IMPLEMENTATION.md         ← Detailed guide
└── REDESIGN_SUMMARY.md              ← Implementation summary
```

---

## 🚀 Quick Migration Guide

### Replacing MudBlazor Components

```razor
<!-- ❌ OLD: MudButton -->
<MudButton Color="Color.Primary" Variant="Variant.Filled">
    Click
</MudButton>

<!-- ✅ NEW: Native + CSS -->
<button class="btn btn-primary">Click</button>

<!-- ❌ OLD: MudPaper -->
<MudPaper Class="pa-4">Content</MudPaper>

<!-- ✅ NEW: Native + CSS -->
<div class="card p-8">Content</div>

<!-- ❌ OLD: MudText -->
<MudText Typo="Typo.h3">Header</MudText>

<!-- ✅ NEW: Native + CSS -->
<h3 class="headline-md">Header</h3>

<!-- ❌ OLD: MudTable -->
<MudTable Items="items"><!-- --></MudTable>

<!-- ✅ NEW: Custom component -->
<table class="design-table"><!-- --></table>
```

---

## 📞 Support

For questions about:
- **Design tokens**: See `app.css` `:root` variables
- **Component usage**: See `DESIGN_IMPLEMENTATION.md`
- **Implementation details**: See `REDESIGN_SUMMARY.md`
- **Original spec**: See `Design.md`

---

**Version**: 1.0  
**System**: The High-Stakes Atelier  
**Status**: ✅ Production Ready
