# Component API Reference

## Complete Developer Reference for Design System Components

---

## DesignButton.razor

### Purpose
Reusable button component with three design variants following the Atelier system.

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ChildContent` | `RenderFragment?` | null | Button text/content |
| `OnClick` | `EventCallback` | - | Click handler |
| `Disabled` | `bool` | `false` | Disable button |
| `Variant` | `string` | `"primary"` | Button style: `"primary"`, `"tertiary"`, `"ghost"` |
| `CssClass` | `string` | `""` | Additional CSS classes |

### Variants

#### Primary (Green)
```razor
<DesignButton Variant="primary" @onclick="SaveData">
    Save Changes
</DesignButton>
```
- Background: var(--secondary) #7dffa2
- Text: var(--on-secondary) #003918
- Hover: Slight opacity change + shadow
- Usage: High-action CTAs

#### Tertiary (No Background)
```razor
<DesignButton Variant="tertiary" @onclick="OpenMenu">
    More Options
</DesignButton>
```
- Background: Transparent
- Text: var(--primary) #bbc6e2
- Hover: var(--surface-container-high) background
- Usage: Secondary actions

#### Ghost (Outline)
```razor
<DesignButton Variant="ghost" @onclick="Cancel">
    Cancel
</DesignButton>
```
- Background: Transparent
- Border: 1px solid rgba(68, 71, 76, 0.15)
- Text: var(--primary) #bbc6e2
- Hover: var(--surface-container-low) background
- Usage: Minimal/dismissive actions

### Events

```razor
<!-- Basic click handler -->
<DesignButton @onclick="HandleClick">Click Me</DesignButton>

<!-- Async operations -->
<DesignButton @onclick="HandleAsync" Disabled="isLoading">
    @(isLoading ? "Loading..." : "Save")
</DesignButton>
```

### Styling

Buttons automatically include:
- Focus states (1px outline)
- Transition animations
- Accessibility features
- Responsive padding

### Examples

```razor
<!-- Action Button -->
<DesignButton Variant="primary" @onclick="SubmitForm">
    ✓ Submit
</DesignButton>

<!-- Conditional Styling -->
<DesignButton Variant="@(IsActive ? "primary" : "tertiary")" @onclick="Toggle">
    Toggle Feature
</DesignButton>

<!-- With Loading State -->
<DesignButton Variant="primary" Disabled="@IsLoading" @onclick="FetchData">
    @(IsLoading ? "Loading..." : "Fetch Data")
</DesignButton>
```

---

## DesignCard.razor

### Purpose
Layered card container without borders, using the surface hierarchy system.

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ChildContent` | `RenderFragment?` | null | Card content |
| `Elevated` | `bool` | `false` | Use elevated styling (floating) |
| `Layer` | `string` | `"low"` | Layer depth: `"low"`, `"medium"`, `"high"`, `"lowest"`, `"highest"` |
| `Padding` | `string` | `"p-8"` | Padding class: `"p-4"`, `"p-6"`, `"p-8"` |
| `CustomClass` | `string?` | null | Additional CSS classes |

### Layer Hierarchy

```
.layer-highest  ← --surface-container-highest (#283646) - Floating overlays
.layer-high     ← --surface-container-high (#1e2b3b)   - Active elements
.layer          ← --surface-container (#132030)        - Primary cards
.layer-low      ← --surface-container-low (#0f1c2c)    - Secondary sections
.layer-lowest   ← --surface-container-lowest (#020f1e) - Recessed data
```

### Usage Examples

#### Standard Card
```razor
<DesignCard Layer="low" Padding="p-8">
    <h2 class="headline-md mb-8">Card Title</h2>
    <p class="body-md">Card content here</p>
</DesignCard>
```

#### Elevated Card (Floating)
```razor
<DesignCard Elevated="true" Layer="highest" Padding="p-8">
    <p>This card appears to float above the surface</p>
</DesignCard>
```

#### Nested Layers (Depth)
```razor
<DesignCard Layer="low" Padding="p-8">
    <h3 class="headline-sm mb-6">Outer Card</h3>
    <DesignCard Layer="lowest" Padding="p-6">
        <p>Nested recessed content</p>
    </DesignCard>
</DesignCard>
```

#### Custom Styling
```razor
<DesignCard Layer="low" Padding="p-8" CustomClass="mt-10 mx-auto" Style="max-width: 500px;">
    <p>Custom card with additional classes</p>
</DesignCard>
```

### Visual Stacking

```
Elevation:    Application Surface
┌─────────────────────────────┐
│ .layer-highest              │ ← Floating modals, FABs
│ ┌─────────────────────────┐ │
│ │ .layer-high             │ │ ← Active selections
│ │ ┌─────────────────────┐ │ │
│ │ │ .layer              │ │ │ ← Primary content
│ │ │ ┌─────────────────┐ │ │ │
│ │ │ │ .layer-low      │ │ │ │ ← Secondary sections
│ │ │ │ ┌─────────────┐ │ │ │ │
│ │ │ │ │ .layer-low  │ │ │ │ │ ← Recessed data
│ │ │ │ └─────────────┘ │ │ │ │
│ │ │ └─────────────────┘ │ │ │
│ │ └─────────────────────┘ │ │
│ └─────────────────────────┘ │
└─────────────────────────────┘
```

### Styling Notes

- No borders (follows "no-line" rule)
- Shadow automatically applied if elevated
- Rounded corners (var(--radius-lg))
- Fully responsive

---

## DesignDataTable.razor

### Purpose
Semantic data table component without divider lines, using spacing and tonal shifts.

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Title` | `string?` | null | Table title |
| `Items` | `IEnumerable<TItem>?` | null | Data rows |
| `HeaderContent` | `RenderFragment?` | null | Table headers (`<th>` elements) |
| `RowTemplate` | `RenderFragment<TItem>?` | null | Row template (receives current item) |

### Generic Type Parameter

```razor
<!-- Generic type must match your data model -->
<DesignDataTable<PlayerViewModel> 
    Title="Leaderboard" 
    Items="players">
    <!-- Headers and rows... -->
</DesignDataTable>
```

### Usage Example

```razor
<DesignDataTable<ParticipantViewModel> 
    Title="Players Scoreboard"
    Items="GetPlayersByTotalValue()">
    
    <HeaderContent>
        <th>#</th>
        <th>Player</th>
        <th>Total Value</th>
        <th>Cash</th>
    </HeaderContent>
    
    <RowTemplate Context="player">
        <td class="data-cell">@player.Rank</td>
        <td>
            <div class="flex-center gap-4" style="justify-content: flex-start;">
                <span>@player.Name</span>
                @if (player.Email == CurrentEmail)
                {
                    <span class="label-md" style="background-color: var(--primary-fixed);">
                        You
                    </span>
                }
            </div>
        </td>
        <td class="data-cell text-secondary">@($"{player.Total:N2} kr")</td>
        <td class="data-cell">@($"{player.Capital:N2} kr")</td>
    </RowTemplate>
</DesignDataTable>
```

### Built-in Styling

| CSS Class | Purpose |
|-----------|---------|
| `.design-table` | Main table container |
| `.data-cell` | Numeric/ticker cells (Work Sans font) |
| `.status-positive` | Green text (growth) |
| `.status-negative` | Red text (loss) |
| `.surface-container-lowest` | Row background (recessed) |

### Special Row Highlighting

```razor
<!-- Highlight specific rows with different backgrounds -->
<RowTemplate Context="player">
    <tr class="@(player.IsCurrent ? "surface-container-high" : "surface-container-lowest")">
        <td>@player.Name</td>
    </tr>
</RowTemplate>
```

### Empty State

```razor
@if (!items.Any())
{
    <div class="card p-12" style="text-align: center;">
        <p class="body-md" style="color: rgba(214, 228, 249, 0.5);">
            No data available
        </p>
    </div>
}
```

### Features

- ✅ No divider lines (spacing-based separation)
- ✅ Hover effects on rows
- ✅ Tonal shifts instead of borders
- ✅ Responsive table layout
- ✅ Accessible semantic markup
- ✅ Data cells use Work Sans font
- ✅ Headers uppercase with green color

---

## GlassmorphicPanel.razor

### Purpose
Modern floating overlay component with glassmorphism effect (60% opacity + 20px blur).

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ChildContent` | `RenderFragment?` | null | Panel content |
| `Title` | `string?` | null | Panel title |
| `HeaderActions` | `RenderFragment?` | null | Actions in header (e.g., close button) |
| `CssClass` | `string` | `""` | Additional CSS classes |

### Effects Applied

```css
/* Automatic effects */
background-color: rgba(40, 54, 70, 0.6);     /* 60% opacity */
backdrop-filter: blur(20px);                 /* Blur background */
border: 1px solid rgba(125, 255, 162, 0.1); /* Subtle green border */
border-radius: var(--radius-lg);             /* Rounded corners */
```

### Basic Usage

```razor
<GlassmorphicPanel Title="Transaction Details">
    <p class="body-md">Your transaction has been processed.</p>
</GlassmorphicPanel>
```

### With Header Actions

```razor
<GlassmorphicPanel Title="Confirm Action" CssClass="mt-10">
    <p class="body-md mb-8">Are you sure you want to proceed?</p>
    
    <HeaderActions>
        <button class="btn btn-tertiary" @onclick="Cancel">Cancel</button>
        <button class="btn btn-primary" @onclick="Confirm">Confirm</button>
    </HeaderActions>
</GlassmorphicPanel>
```

### Overlay Background

For full-screen modal effect, wrap with overlay div:

```html
<div style="position: fixed; inset: 0; display: flex; align-items: center; justify-content: center; z-index: 1000;">
    <div style="background-color: rgba(2, 15, 30, 0.8); backdrop-filter: blur(20px); border-radius: var(--radius-lg); padding: var(--spacing-8);">
        <GlassmorphicPanel Title="Modal Title">
            <p>Modal content</p>
        </GlassmorphicPanel>
    </div>
</div>
```

### Advanced: Form in Panel

```razor
<GlassmorphicPanel Title="Add Stock Transaction">
    <form @onsubmit="HandleSubmit">
        <div class="mb-8">
            <label class="label-md text-secondary mb-2">Ticker Symbol</label>
            <input type="text" @bind="ticker" class="w-100" />
        </div>
        
        <div class="mb-8">
            <label class="label-md text-secondary mb-2">Quantity</label>
            <input type="number" @bind="quantity" class="w-100" />
        </div>
        
        <HeaderActions>
            <button type="button" class="btn btn-tertiary" @onclick="Cancel">
                Cancel
            </button>
            <button type="submit" class="btn btn-primary">
                Add Transaction
            </button>
        </HeaderActions>
    </form>
</GlassmorphicPanel>
```

### Styling Considerations

- Title uses `.headline-sm` automatically
- Content has generous padding
- Actions are right-aligned
- Responds to light/dark mode (dark only in current design)

---

## Utility Classes Reference

### Layout Classes

```
.flex-center        /* display: flex; align-items: center; justify-content: center; */
.flex-between       /* justify-content: space-between; */
.flex-column        /* flex-direction: column; */

.gap-2 / .gap-4 / .gap-6 / .gap-8
.gap-breathing      /* var(--spacing-10) */
.gap-breathing-lg   /* var(--spacing-12) */
```

### Spacing Classes

```
Padding:
.p-4 / .p-6 / .p-8
.px-8               /* left & right */
.py-8               /* top & bottom */

Margin:
.mb-4 / .mb-6 / .mb-8   /* margin-bottom */
.mt-8 / .mt-10
.mx-auto            /* auto left & right */
```

### Color Classes

```
.text-primary       /* var(--primary) */
.text-secondary     /* var(--secondary) */
.text-error         /* var(--error) */
.text-surface       /* var(--on-surface) */

.surface-container-low
.surface-container
.surface-container-high
.surface-container-highest
.surface-container-lowest
```

### Special Classes

```
.data-cell          /* Work Sans, green color, bold */
.status-positive    /* Green text */
.status-negative    /* Red text */

.card               /* Styled card container */
.card-elevated      /* Floating card */

.glass-panel        /* Glassmorphism effect */
.glass-panel-overlay /* Dark overlay for modal */

.w-100              /* width: 100% */
```

---

## Common Patterns

### Action Button with Loading

```razor
<DesignButton 
    Variant="@(isLoading ? "tertiary" : "primary")" 
    Disabled="@isLoading" 
    @onclick="HandleAction">
    @if (isLoading)
    {
        <span>⏳ Loading...</span>
    }
    else
    {
        <span>Save Changes</span>
    }
</DesignButton>
```

### Conditional Button State

```razor
<DesignButton 
    Variant="@(IsSelected ? "primary" : "tertiary")" 
    @onclick="ToggleSelection">
    @(IsSelected ? "✓ Selected" : "Select")
</DesignButton>
```

### Nested Card Hierarchy

```razor
<DesignCard Layer="low">
    <DesignCard Layer="medium">
        <DesignCard Layer="highest">
            Content with visual depth
        </DesignCard>
    </DesignCard>
</DesignCard>
```

### Table with Sorting

```razor
<DesignDataTable Items="SortedItems">
    <HeaderContent>
        <th @onclick="() => Sort("name")">
            Player @GetSortIndicator("name")
        </th>
    </HeaderContent>
</DesignDataTable>
```

---

## Best Practices

### ✅ DO

```razor
<!-- Use semantic classes -->
<button class="btn btn-primary">Good</button>

<!-- Use layer system consistently -->
<DesignCard Layer="low">Consistent</DesignCard>

<!-- Use design tokens -->
<div style="padding: var(--spacing-8);">Correct</div>

<!-- Use typography classes -->
<h2 class="headline-md">Proper</h2>
```

### ❌ DON'T

```razor
<!-- Hardcoded colors -->
<button style="background-color: #7dffa2;">Bad</button>

<!-- Arbitrary padding -->
<div style="padding: 15px;">Inconsistent</div>

<!-- Mixed font systems -->
<p style="font-family: Arial;">Wrong</p>

<!-- Border dividers -->
<div style="border-bottom: 1px solid gray;">Old pattern</div>
```

---

## Responsive Considerations

All components are:
- ✅ Mobile-first
- ✅ Fully responsive
- ✅ Flexible layouts
- ✅ Touch-friendly
- ✅ No media queries needed (uses flexbox)

---

## Accessibility Features

All components include:
- ✅ Focus indicators (1px outline)
- ✅ Semantic HTML
- ✅ Proper heading hierarchy
- ✅ Color contrast (WCAG AA)
- ✅ Keyboard navigation

---

**Reference Version**: 1.0  
**Last Updated**: Implementation Complete  
**System**: The High-Stakes Atelier
