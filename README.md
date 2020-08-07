# wpf-uwp-icons

This project contains an [SVG to XML converter](converter/README.md), all of the [Material Design Icons](https://material.io/resources/icons) in a [ResourceDictionary](icons/ResourceDictionary_Icons.xaml), and examples of their usage in [WPF](samples/WPF/README.md) and [UWP](samples/UWP/README.md) applications.

The converter app uses [vvvv's SVG library](https://github.com/vvvv/SVG) to parse the SVG xml. It then converts the svg elements to their corresponding XAML element, places them on a canvas, inside of a `Viewbox`, and then inside of a `ControlTemplate`. These are all added to a `ResourceDictionary.xaml` and can be used easily in buttons.

```xaml
<Button>
    <Button.Content>
        <ContentControl Template="{StaticResource IconKeyHere}" />
    </Button.Content>
</Button>
```

I processed all of the [Material Design icons](https://material.io/resources/icons) and placed them in a resource dictionary [here](icons/ResourceDictionary_Icons.xaml).

## Sample library

![UWP sample](/screenshots/uwp.gif)
