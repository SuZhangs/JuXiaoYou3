# Xaml 部分

``` xaml
<Border.Background>
    <ImageBrush>
        <ImageBrush.ImageSource>
            <MultiBinding Converter="{StaticResource AvatarConverter}">
                <Binding Path="Avatar" Mode="OneWay" />
                <Binding Path="Type" />
        </MultiBinding>
    </ImageBrush.ImageSource>
</ImageBrush>
```
# 转换器声明


``` xaml            
 xmlns:thisConverters="clr-namespace:Acorisoft.FutureGL.MigaStudio.Resources.Converters"
 <thisConverters:AvatarConverter x:Key="AvatarConverter" />
```