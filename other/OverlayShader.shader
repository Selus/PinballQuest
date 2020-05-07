shader_type canvas_item;
render_mode unshaded;

uniform sampler2D displace : hint_albedo;
uniform float dispAmt : hint_range(0, 0.1);
uniform float dispScale : hint_range(0.1, 2.0);
uniform float abberationAmt : hint_range(0, 0.1);
uniform float timeLine : hint_range(0.0, 10.0);

uniform float scanSpeed : hint_range(0.0, 100.0);
uniform float scanOffset : hint_range(0.0, 100.0);

uniform float screenCurvature : hint_range(0.0, 10.0);
uniform float curvatureDistance : hint_range(0.0, 10.0);

vec2 distort(vec2 p) {

	float theta = atan(p.y, p.x);
	float radius = length(p);
	radius = pow(radius, screenCurvature);
	
	p.x = radius * cos(theta);
	p.y = radius * sin(theta);
	
	return 0.5 * (p + vec2(1.0,1.0));
}


void fragment() {
	
	// Remaping from 0:1 to -1:1
	vec2 p = (2.0 * SCREEN_UV) - 1.0;
	float d = length(p);
	if(d < curvatureDistance){
		p = distort(p);
	}else{
		p = distort(p);
	}
	
	vec2 noiseUV = p;
	
	// Abberation
	COLOR.r = texture(SCREEN_TEXTURE, noiseUV - abberationAmt).r;
	COLOR.g = texture(SCREEN_TEXTURE, noiseUV).g;
	COLOR.b = texture(SCREEN_TEXTURE, noiseUV + abberationAmt).b;
	COLOR.a = texture(SCREEN_TEXTURE, noiseUV).a;

}