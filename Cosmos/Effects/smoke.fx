float time;
Texture2D<float4> noiseTexture;

sampler TexSampler : register(s0);

sampler noiseSampler = sampler_state
{
	Texture = <noiseTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 texCoord : TEXCOORD0;
};

float noise(float2 x)
{
	float4 tex = tex2D(noiseSampler, x * 0.01);
	return tex.x;
}

float2x2 makem2(in float theta)
{
	float c = cos(theta);
	float s = sin(theta);
	return float2x2(c, -s, s, c);
}

float2 gradn(float2 p)
{
	float ep = 0.09;
	float gradx = noise(float2(p.x + ep, p.y)) - noise(float2(p.x - ep, p.y));
	float grady = noise(float2(p.x, p.y + ep)) - noise(float2(p.x, p.y - ep));
	return float2(gradx, grady);
}

float flow(in float2 p)
{
	float z = 2;
	float rz = 0;
	float2 bp = p;
	float utime = time * 0.1;

	for (float i = 1.; i < 7.; i++)
	{
		//primary flow speed
		p += utime * .6;

		//secondary flow speed
		bp += utime * 1.9;

		//displacement field
		float2 gr = gradn(i * p * .34 + utime * 0.01);

		//rotation of the displacement field
		gr = mul(gr, makem2(utime * 6. - (0.05 * p.x + 0.03 * p.y) * 40.));

		//displace the system
		p += gr * .5;

		//add noise octave
		rz += (sin(noise(p) * 7.) * 0.5 + 0.5) / z;

		//blend factor
		p = lerp(bp, p, .77);

		//intensity scaling
		z *= 1.4;

		//octave scaling
		p *= 2;
		bp *= 1.9;
	}

	return rz;
}

float4 PSMain(VertexShaderOutput input) : COLOR
{
	float2 p = input.texCoord.xy;// / float2(100, 100).xy;
	p *= 3;
	float rz = flow(p);

	float3 col = float3(.2, 0.07, 0.01) / rz;
	col = pow(col, 1.4);
	return float4(col, 1.0);
	//return tex2D(TexSampler, input.texCoord) * float4(col, 1.0);
}

technique Lava
{
	pass P0
	{
		//VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile ps_3_0 PSMain();
	}
};