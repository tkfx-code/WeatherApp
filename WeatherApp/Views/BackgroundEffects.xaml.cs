using Particle.Maui.ParticleGenerators;
using Particle.Maui;

namespace WeatherApp.Views;
public partial class BackgroundEffects : ContentView
{
	public BackgroundEffects()
	{
		InitializeComponent();
		SetupRain();
	}

	private void SetupRain()
	{
		RainEffect.HasFallingParticles = true;
        RainEffect.FallingParticlesPerSecond = 80;
        RainEffect.FallingParticleGenerator = new FallingParticleGenerator();
    }
}