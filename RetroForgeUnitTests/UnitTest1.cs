using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroForge.NET;
using RetroForge.NET.Assets;
using RetroForge.NET.Assets.Asset;
using RetroForge.NET.Graphics;
using SixLabors.Fonts;

namespace RetroForgeUnitTests
{
    public class AssetLoaderTests
    {
        AssetLoader? loader;
        [SetUp]
        public void Setup()
        {
            loader = new AssetLoader(@$"C:\Users\{Environment.UserName}\AppData\Roaming\RetroForge\Assets\TestZip.zip");
        }

        
        private void TestAssetLoaderMethods()
        {
            loader = new AssetLoader(@$"C:\Users\{Environment.UserName}\AppData\Roaming\RetroForge\Assets\TestZip.zip");

            if (loader is null) Assert.Fail("AssetLoader is null");
            bool[] Success = [false, false];

            Success[0] = LoadAllAssets();
            Success[1] = GetAsset();

            if (Success.All((b0) => b0)) { Assert.Pass("AssetLoader Passed"); } else Assert.Fail($"AssetLoader Failed {Success[0]} {Success[1]}");

        }

        [Test(ExpectedResult = true), Order(0)]
        public bool LoadAllAssets()
        {
            loader.LoadAllAssets();

            bool Fail = false;

            Fail |= loader.NAssets != 3;

            loader.UnloadAllAssets();
            return !Fail;

        }
        [Test(ExpectedResult = true), Order(1)]
        public bool GetAsset()
        {
            
            loader.LoadAllAssets();

            IAsset asset = loader!.GetAsset(0);
            bool Fail = false;
            Fail |= asset.AssetType != AssetType.Text;
            Fail |= ((Text)asset)._Text != "lorem ipsum dolor";
            loader.UnloadAllAssets();
            return !Fail;
        }

        [Test, Order(2)]
        public void ToLocal()
        {
            if (AssetLoader.ToLocal(@"test/test/test.zip/test") == @"test/test/test/test") Assert.Pass("AssetLoader.ToLocal Passed"); else Assert.Fail("AssetLoader.ToLocal Failed");
        }
    }
    public class ShaderTests
    {
        class BindingsContext : IBindingsContext
        {
            public nint GetProcAddress(string procName)
            {
                return GLFW.GetProcAddress(procName);
            }
        }


        static readonly string testVertSource =
"""
#version 460
void main() {
    gl_Position = vec4(0.0, 0.0, 0.0, 1.0);
}
""";
        static readonly string testFragSource =
"""
#version 460
layout (location = 0) out vec4 fragColour;
void main() {
    fragColour = vec4(0.0, 0.0, 0.0, 1.0);
}
""";
        [SetUp]
        public void Setup()
        {
        }

        [Test(ExpectedResult = true)]
        public bool CompileVertShader()
        {
            GLLoader.LoadBindings(new BindingsContext());
            Shader? shader = Shader.CompileShader(testVertSource, @"C:\Users\cam33\AppData\Roaming\RetroForge\ShaderTesting\base-vert.spv", shaderc.SourceLanguage.Glsl, shaderc.ShaderKind.VertexShader);
            bool success = shader is not null;
            return success;
        }

        [Test(ExpectedResult = true)]
        public bool CompileFragShader()
        {
            GLLoader.LoadBindings(new BindingsContext());
            Shader? shader = Shader.CompileShader(testFragSource, @"C:\Users\cam33\AppData\Roaming\RetroForge\ShaderTesting\base-frag.spv", shaderc.SourceLanguage.Glsl, shaderc.ShaderKind.FragmentShader);
            bool success = shader is not null;
            return success;
        }
    }
    public class StyledStringTests
    {
        [Test(ExpectedResult = true)]
        public bool Render()
        {
            StyledString sstr = new();
            Engine.AddSystemFonts();
            sstr.Render("hello world", "Arial\n12\n0/1/1\n");
            Console.WriteLine("if sstr rendered correctly hit enter");

            return true;//Console.ReadKey().Key == ConsoleKey.Enter;
        }

        [Test(ExpectedResult = true)]
        public bool FontColor()
        {
            var col = StyledString.FontColour("0/1/1");
            return (col.X == 0) && (col.Y == 1) && (col.Z == 1);
        }
    }
}
